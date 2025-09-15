using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Board
{
    public int Turn;
    public Piece[] Pieces;
    public Piece[,] Squares;
    public readonly Move? LastMove;
    public bool[] CastleQueenSide;
    public bool[] CastleKingSide;
    public Dictionary<byte, IItem[]> ItemsPerPiece;

    public Board LastBoard;
    public Piece EnPassantPawn;
    public uint ZobristHash;
    
    private Dictionary<byte, Piece> pieceDict = [];


    public bool ColorToMove => Turn % 2 == 0;
    public int ColorIndex => Turn % 2;

    public Board(
        int turn,
        Piece[] pieces,
        bool[] castleQueenSide,
        bool[] castleKingSide,
        Dictionary<byte, IItem[]> itemsPerPiece,
        Move? lastMove = null,
        Board lastBoard = null,
        uint zobristHash = 0
    )
    {
        Turn = turn;
        Pieces = pieces;

        CastleQueenSide = castleQueenSide;
        CastleKingSide = castleKingSide;
        ItemsPerPiece = itemsPerPiece;
        LastMove = lastMove;
        LastBoard = lastBoard;
        
        Squares = new Piece[8, 8];
        foreach (Piece piece in pieces)
        {
            Squares.Set(piece.Position, piece);
            pieceDict[piece.Id] = piece;
            if (piece.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
            {
                EnPassantPawn = piece;
            }
        }

        ZobristHash = zobristHash;
        if (ZobristHash == 0)
        {
            ZobristHash = GetZobristHash();
        }
    }

    public static Board DefaultBoard()
    {
        Piece[] pieces =
        [
            // White
            Piece.Pawn(0, true, new Vector2Int(0, 1)),
            Piece.Pawn(1, true, new Vector2Int(1, 1)),
            Piece.Pawn(2, true, new Vector2Int(2, 1)),
            Piece.Pawn(3, true, new Vector2Int(3, 1)),
            Piece.Pawn(4, true, new Vector2Int(4, 1)),
            Piece.Pawn(5, true, new Vector2Int(5, 1)),
            Piece.Pawn(6, true, new Vector2Int(6, 1)),
            Piece.Pawn(7, true, new Vector2Int(7, 1)),

            Piece.Rook(8, true, new Vector2Int(0, 0), SpecialPieceTypes.QUEEN_SIDE_CASTLE),
            Piece.Knight(9, true, new Vector2Int(1, 0)),
            Piece.Bishop(10, true, new Vector2Int(2, 0)),
            Piece.Queen(11, true, new Vector2Int(3, 0)),
            Piece.King(12, true, new Vector2Int(4, 0), SpecialPieceTypes.KING),
            Piece.Bishop(13, true, new Vector2Int(5, 0)),
            Piece.Knight(14, true, new Vector2Int(6, 0)),
            Piece.Rook(15, true, new Vector2Int(7, 0), SpecialPieceTypes.KING_SIDE_CASTLE),

            // Black
            Piece.Pawn(16, false, new Vector2Int(0, 6)),
            Piece.Pawn(17, false, new Vector2Int(1, 6)),
            Piece.Pawn(18, false, new Vector2Int(2, 6)),
            Piece.Pawn(19, false, new Vector2Int(3, 6)),
            Piece.Pawn(20, false, new Vector2Int(4, 6)),
            Piece.Pawn(21, false, new Vector2Int(5, 6)),
            Piece.Pawn(22, false, new Vector2Int(6, 6)),
            Piece.Pawn(23, false, new Vector2Int(7, 6)),

            Piece.Rook(24, false, new Vector2Int(0, 7), SpecialPieceTypes.KING_SIDE_CASTLE),
            Piece.Knight(25, false, new Vector2Int(1, 7)),
            Piece.Bishop(26, false, new Vector2Int(2, 7)),
            Piece.Queen(27, false, new Vector2Int(3, 7)),
            Piece.King(28, false, new Vector2Int(4, 7), SpecialPieceTypes.KING),
            Piece.Bishop(29, false, new Vector2Int(5, 7)),
            Piece.Knight(30, false, new Vector2Int(6, 7)),
            Piece.Rook(31, false, new Vector2Int(7, 7), SpecialPieceTypes.QUEEN_SIDE_CASTLE),
        ];

        return new Board(0, pieces, [true, true], [true, true], []);
    }

    public Board Copy(Move? lastMove = null)
    {
        Piece[] pieces = new Piece[Pieces.Length];
        uint zobrist = ZobristHash;
        // Array.Copy(Pieces, pieces, Pieces.Length);
        for (int i = 0; i < Pieces.Length; i++)
        {
            Piece toCopy = Pieces[i];
            
            pieces[i] = toCopy.DeepCopy();
            if (toCopy.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
            {
                // Reflect en passant decay in the zobrist hash
                zobrist ^= ZobristCalculator.GetZobristHash(toCopy.Color, toCopy.Position, SpecialPieceTypes.EN_PASSANTABLE_PAWN);
                zobrist ^= ZobristCalculator.GetZobristHash(toCopy.Color, toCopy.Position, toCopy.BasePiece == BasePiece.PAWN ? SpecialPieceTypes.PAWN : SpecialPieceTypes.NONE);
            }
        }
        return new Board(Turn, pieces, [CastleQueenSide[0], CastleQueenSide[1]], [CastleKingSide[0], CastleKingSide[1]], ItemsPerPiece, lastMove, this, zobrist);
    }

    public Piece GetPiece(byte id)
    {
        if (pieceDict.TryGetValue(id, out Piece result))
            return result;
        return null;
    }

    public bool AddPiece(Piece piece)
    {
        if (pieceDict.ContainsKey(piece.Id))
            return false;
        
        Piece[] newPieces = new Piece[Pieces.Length + 1];
        Pieces.CopyTo(newPieces, 0);
        newPieces[^1] = piece;
        Pieces = newPieces;
        Squares.Set(piece.Position, piece);
        pieceDict.Add(piece.Id, piece);

        ZobristCalculator.AdjustZobristHash(piece, this);

        return true;
    }

    public bool RemovePiece(byte id)
    {
        if (!pieceDict.TryGetValue(id, out Piece toRemove))
            return false;

        int index = 0;
        Piece[] newPieces = new Piece[Pieces.Length - 1];
        foreach (Piece piece in Pieces)
        {
            if (piece.Id == id)
                continue;
            newPieces[index] = piece;
            index++;
        }
        Pieces = newPieces;
        pieceDict.Remove(id);
        if (Squares.Get(toRemove.Position) == toRemove)
            Squares.Set(toRemove.Position, null);
        
        ZobristCalculator.AdjustZobristHash(toRemove, this);

        return true;
    }

    public void ActivateItems(bool color, ItemTriggers trigger, Board board, Move move, IBoardEvent triggerEvent)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece.Color != color)
                continue;
            board.ActivateItems(piece.Id, trigger, board, move, triggerEvent);
        }
    }

    public void ActivateItems(ItemTriggers trigger, Board board, Move move, IBoardEvent triggerEvent)
    {
        foreach (KeyValuePair<byte, IItem[]> pair in board.ItemsPerPiece)
        {
            board.ActivateItems(pair.Key, trigger, board, move, triggerEvent);
        }
    }

    public void ActivateItems(byte pieceId, ItemTriggers trigger, Board board, Move move, IBoardEvent triggerEvent)
    {
        if (board.ItemsPerPiece.TryGetValue(pieceId, out IItem[] items))
        {
            foreach (IItem item in items)
            {
                if (item.Trigger == trigger && item.ConditionsMet(board, move, triggerEvent))
                {
                    board = item.Execute(board, move, triggerEvent);
                }
            }
        }
    }

    public bool IsInCheck(bool color)
    {
        Vector2Int kingPosition = new(0, 0);
        foreach (Piece piece in Pieces)
        {
            if (piece.SpecialPieceType != SpecialPieceTypes.KING || piece.Color != color)
                continue;
            kingPosition = piece.Position;
            break;
        }

        return IsInCheck(color, kingPosition);
    }

    public bool IsInCheck(bool color, Vector2Int position)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece.Color == color)
                continue;
            foreach (IMovement movement in piece.Movement)
            {
                if (movement.Attacks(piece.Position, position, this, color))
                    return true;
            }
        }

        return false;
    }

    public bool IsInCheck(bool color, Vector2Int[] positions)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece.Color == color)
                continue;
            foreach (IMovement movement in piece.Movement)
                if (movement.AttacksAny(piece.Position, positions, this, color))
                    return true;
        }

        return false;
    }

    public List<Move> GetMoves()
    {
        List<Move> result = [];
        foreach (Piece piece in Pieces)
        {
            if (piece.Color != ColorToMove)
                continue;
            foreach (Move move in piece.GetMovementOptions(this))
            {
                if (!move.Result.IsInCheck(ColorToMove))
                {
                    result.Add(move);
                }
            }
        }
        return result;
    }

    public List<Move> GetMoves(Piece piece)
    {
        List<Move> result = [];
        
        foreach (Move move in piece.GetMovementOptions(this))
        {
            if (!move.Result.IsInCheck(piece.Color))
            {
                result.Add(move);
            }
        }

        return result;
    }

    public Piece[] DeepcopyPieces(params byte[] idToSkip)
    {
        // Result leaves 1 "empty" spot in the array (to be filled with the moved piece)
        Piece[] result = new Piece[Pieces.Length - idToSkip.Length + 1];
        int i = 0;
        foreach (Piece p in Pieces)
        {
            // Remove this piece from list, add it back with new position
            // Remove captures piece if applicable
            if (idToSkip.Contains(p.Id))
                continue;
            result[i] = p.DeepCopy();
            i++;
        }

        return result;
    }

    public uint GetZobristHash()
    {
        // if (ZobristHash != 0)
        //     return ZobristHash;
        
        uint result = 0;

        foreach (Piece piece in Pieces)
        {
            // Hash for piece (including base piece, special type, all movements)
            result ^= piece.GetZobristHash();
            
            if (ItemsPerPiece.TryGetValue(piece.Id, out IItem[] items))
            {
                foreach (IItem item in items)
                {
                    // Hash for every item
                    result ^= item.GetZobristHash(piece.Color, piece.Position);
                }
            }
        }
        // For castling rights
        result ^= ZobristCalculator.GetZobristHash(CastleKingSide, CastleQueenSide);
        // Side to move
        result ^= ZobristCalculator.GetZobristHash(ColorToMove);
        
        return result;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Board board)
            return false;
        if (!board.CastleKingSide.SequenceEqual(CastleKingSide))
            return false;
        if (!board.CastleQueenSide.SequenceEqual(CastleQueenSide))
            return false;

        string ownFEN = FENConverter.BoardToFEN(this, false);
        string otherFEN = FENConverter.BoardToFEN(board, false);
        if (ownFEN == otherFEN)
            return true;

        return false;
    }

    public override string ToString()
    {
        if (LastMove is null)
            return "Root board";
        return LastMove.ToString();
    }
}
