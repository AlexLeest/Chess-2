using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Board
{
    public Board LastBoard;
    
    public Piece[] Pieces;
    public Piece[,] Squares;
    public readonly int Turn;
    public readonly Move? LastMove;
    public bool[] CastleQueenSide;
    public bool[] CastleKingSide;

    public readonly Dictionary<byte, IItem[]> ItemsPerPiece;

    public bool ColorToMove => Turn % 2 == 0;

    public Board(
        int turn,
        Piece[] pieces,
        bool[] castleQueenSide,
        bool[] castleKingSide,
        Dictionary<byte, IItem[]> itemsPerPiece,
        Move? lastMove = null,
        Board lastBoard = null
    )
    {
        Turn = turn;
        Pieces = pieces;

        this.CastleQueenSide = castleQueenSide;
        this.CastleKingSide = castleKingSide;
        this.ItemsPerPiece = itemsPerPiece;
        this.LastMove = lastMove;
        this.LastBoard = lastBoard;
        
        Squares = new Piece[8, 8];
        foreach (Piece piece in pieces)
        {
            Squares[piece.Position.X, piece.Position.Y] = piece;
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
        
        Dictionary<byte, IItem[]> itemsPerPiece = new();

        return new Board(0, pieces, [true, true], [true, true], itemsPerPiece);
    }

    public Piece GetPiece(byte id)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece.Id != id)
                continue;
            return piece;
        }
        return null;
    }

    public Board ActivateItems(bool color, ItemTriggers trigger, Board board, Move move)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece.Color != color)
                continue;
            board = board.ActivateItems(piece.Id, trigger, board, move);
        }
        return board;
    }

    public Board ActivateItems(ItemTriggers trigger, Board board, Move move)
    {
        foreach (var pair in board.ItemsPerPiece)
        {
            board = board.ActivateItems(pair.Key, trigger, board, move);
        }
        return board;
    }

    public Board ActivateItems(byte pieceId, ItemTriggers trigger, Board board, Move move)
    {
        if (board.ItemsPerPiece.TryGetValue(pieceId, out IItem[] captureItems))
        {
            foreach (IItem item in captureItems)
            {
                if (item.Trigger == trigger && item.ConditionsMet(board, move))
                {
                    board = item.Execute(board, move);
                }
            }
        }
        return board;
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

    private bool IsInCheck(bool color, Vector2Int[] positions)
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

    public List<Board> GenerateMoves()
    {
        bool colorToMove = Turn % 2 == 0;
        List<Board> result = [];
        foreach (Piece piece in Pieces)
        {
            if (piece.Color != colorToMove)
                continue;
            foreach (Board move in GenerateMoves(piece))
            {
                result.Add(move);
            }
        }
        return result;
    }

    public List<Board> GenerateMoves(Piece piece)
    {
        bool colorToMove = Turn % 2 == 0;
        int colorIndex = Turn % 2;
        int nextTurn = Turn + 1;
        List<Board> result = [];

        // For each possible move
        foreach (Move move in piece.GetMovementOptions(this))
        {
            bool promotion = false;
            Piece capturedPiece = move.Captured;

            // Make full copy of all unmoved pieces
            Piece[] newPieces;
            if (capturedPiece == null)
                newPieces = DeepcopyPieces(piece.Id);
            else
                newPieces = DeepcopyPieces(piece.Id, capturedPiece.Id);

            // Add moved piece (in new position) back to the list
            Piece newPiece;
            if (piece.SpecialPieceType == SpecialPieceTypes.PAWN && move.To.Y == (piece.Color ? 7 : 0))
            {
                // Promotion! Just to queen for now
                // TODO: Promotion to bishop, rook, knight
                promotion = true;
                newPiece = new Piece(piece.Id, BasePiece.QUEEN, piece.Color, move.To, [SlidingMovement.Queen]);
            }
            else if (piece.SpecialPieceType == SpecialPieceTypes.PAWN && Math.Abs(move.To.Y - piece.Position.Y) == 2)
            {
                newPiece = new Piece(piece.Id, piece.BasePiece, piece.Color, move.To, piece.Movement, SpecialPieceTypes.EN_PASSANTABLE_PAWN);
            }
            else
            {
                newPiece = new Piece(piece.Id, piece.BasePiece, piece.Color, move.To, piece.Movement, piece.SpecialPieceType);
            }
            newPieces[^1] = newPiece;

            // If any of the castling pieces move, disallow castling in future boards
            bool[] newCastleQueenSide = [CastleQueenSide[0], CastleQueenSide[1]];
            bool[] newCastleKingSide = [CastleKingSide[0], CastleKingSide[1]];
            switch (piece.SpecialPieceType)
            {
                case SpecialPieceTypes.KING:
                    newCastleQueenSide[colorIndex] = false;
                    newCastleKingSide[colorIndex] = false;
                    break;
                case SpecialPieceTypes.QUEEN_SIDE_CASTLE:
                    newCastleQueenSide[colorIndex] = false;
                    break;
                case SpecialPieceTypes.KING_SIDE_CASTLE:
                    newCastleKingSide[colorIndex] = false;
                    break;
            }
            if (capturedPiece is not null)
            {
                int otherColorIndex = nextTurn % 2;
                switch (capturedPiece.SpecialPieceType)
                {
                    case SpecialPieceTypes.QUEEN_SIDE_CASTLE:
                        newCastleQueenSide[otherColorIndex] = false;
                        break;
                    case SpecialPieceTypes.KING_SIDE_CASTLE:
                        newCastleKingSide[otherColorIndex] = false;
                        break;
                }
            }

            // Make new board add to results
            // Move committedMove = new(piece.Id, piece.Position, move, capturedPiece);
            Board possibleMove = new(nextTurn, newPieces, newCastleQueenSide, newCastleKingSide, ItemsPerPiece, move, this);
            
            // Trigger ON_MOVE items
            possibleMove = ActivateItems(piece.Id, ItemTriggers.ON_MOVE, possibleMove, move);
            
            if (capturedPiece is not null)
            {
                // Trigger ON_CAPTURE and ON_CAPTURED items
                possibleMove = possibleMove.ActivateItems(piece.Id, ItemTriggers.ON_CAPTURE, possibleMove, move);
                possibleMove = possibleMove.ActivateItems(capturedPiece.Id, ItemTriggers.ON_CAPTURED, possibleMove, move);
            }
            if (promotion)
            {
                // Trigger ON_PROMOTION items if that flag is set
                possibleMove = possibleMove.ActivateItems(piece.Id, ItemTriggers.ON_PROMOTION, possibleMove, move);
            }
            
            // Trigger all ON_TURN items
            possibleMove = possibleMove.ActivateItems(colorToMove, ItemTriggers.ON_TURN, possibleMove, move);
            
            if (!possibleMove.IsInCheck(colorToMove))
                result.Add(possibleMove);
        }

        // Castling, separate from any preset movements
        if (piece.SpecialPieceType == SpecialPieceTypes.KING && !IsInCheck(colorToMove))
        {
            int colorRank = colorToMove ? 0 : 7;
            if (CastleKingSide[colorIndex])
            {
                // IF there's no pieces on files 5, 6
                // AND no possible capture on files 5, 6
                // Move king to file 6
                // Move piece on file 7 to file 5
                if (
                    Squares[5, colorRank] == null && Squares[6, colorRank] == null &&
                    !IsInCheck(colorToMove, [new Vector2Int(5, colorRank), new Vector2Int(6, colorRank)])
                )
                {
                    Piece toCastle = Squares[7, colorRank];
                    if (toCastle is not null && toCastle.SpecialPieceType == SpecialPieceTypes.KING_SIDE_CASTLE)
                    {
                        Piece[] newPieces = CastleDeepcopy(piece.Id, toCastle.Id);
                        // Move king
                        newPieces[^2] = new Piece(piece.Id, piece.BasePiece, piece.Color, new Vector2Int(6, colorRank), piece.Movement, piece.SpecialPieceType);
                        // Move piece
                        newPieces[^1] = new Piece(toCastle.Id, toCastle.BasePiece, toCastle.Color, new Vector2Int(5, colorRank), toCastle.Movement, toCastle.SpecialPieceType);

                        // Remove castling rights
                        bool[] newCastleQueenSide = [CastleQueenSide[0], CastleQueenSide[1]];
                        bool[] newCastleKingSide = [CastleKingSide[0], CastleKingSide[1]];
                        newCastleQueenSide[colorIndex] = false;
                        newCastleKingSide[colorIndex] = false;
                        Move castleMove = new(piece.Id, piece.Position, new Vector2Int(6, colorRank));
                        Board castledBoard = new(nextTurn, newPieces, newCastleQueenSide, newCastleKingSide, ItemsPerPiece, castleMove, this);

                        // Activate any possible ON_CASTLE/ON_OPPONENT_CASTLE items
                        foreach (Piece toActivate in castledBoard.Pieces)
                        {
                            ItemTriggers trigger = toActivate.Color == colorToMove ? ItemTriggers.ON_CASTLE : ItemTriggers.ON_OPPONENT_CASTLE;
                            castledBoard = castledBoard.ActivateItems(toActivate.Id, trigger, castledBoard, castleMove);
                        }

                        castledBoard = castledBoard.ActivateItems(ItemTriggers.ON_TURN, castledBoard, castleMove);
                        // Add to results
                        result.Add(castledBoard);
                    }
                }
            }
            if (CastleQueenSide[colorIndex])
            {
                // IF there's no pieces on files 1, 2, 3
                // AND no possible capture on files 2, 3
                // Move king to file 2
                // Move piece on file 0 to file 3
                if (
                    Squares[1, colorRank] == null && Squares[2, colorRank] == null && Squares[3, colorRank] == null &&
                    !IsInCheck(colorToMove, [new Vector2Int(2, colorRank), new Vector2Int(3, colorRank)])
                )
                {
                    Piece toCastle = Squares[0, colorRank];
                    if (toCastle is not null && toCastle.SpecialPieceType == SpecialPieceTypes.QUEEN_SIDE_CASTLE)
                    {
                        Piece[] newPieces = CastleDeepcopy(piece.Id, toCastle.Id);
                        // Move king
                        newPieces[^2] = new Piece(piece.Id, piece.BasePiece, piece.Color, new Vector2Int(2, colorRank), piece.Movement, piece.SpecialPieceType);
                        // Move piece
                        newPieces[^1] = new Piece(toCastle.Id, toCastle.BasePiece, toCastle.Color, new Vector2Int(3, colorRank), toCastle.Movement, toCastle.SpecialPieceType);

                        // Remove castling rights
                        bool[] newCastleQueenSide = [CastleQueenSide[0], CastleQueenSide[1]];
                        bool[] newCastleKingSide = [CastleKingSide[0], CastleKingSide[1]];
                        newCastleQueenSide[colorIndex] = false;
                        newCastleKingSide[colorIndex] = false;
                        Move castleMove = new(piece.Id, piece.Position, new Vector2Int(2, colorRank));
                        Board castledBoard = new(nextTurn, newPieces, newCastleQueenSide, newCastleKingSide, ItemsPerPiece, castleMove, this);

                        // Activate any possible ON_CASTLE/ON_OPPONENT_CASTLE items
                        foreach (Piece toActivate in castledBoard.Pieces)
                        {
                            ItemTriggers trigger = toActivate.Color == colorToMove ? ItemTriggers.ON_CASTLE : ItemTriggers.ON_OPPONENT_CASTLE;
                            castledBoard = castledBoard.ActivateItems(toActivate.Id, trigger, castledBoard, castleMove);
                        }

                        castledBoard = castledBoard.ActivateItems(ItemTriggers.ON_TURN, castledBoard, castleMove);
                        // Add to results
                        result.Add(castledBoard);
                    }
                }
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
            // Piece deepCopy = new(p.Id, p.BasePiece, p.Color, p.Position, p.Movement, p.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN ? SpecialPieceTypes.PAWN : p.SpecialPieceType);
            // result[i] = deepCopy;
            result[i] = p.DeepCopy();
            i++;
        }

        return result;
    }

    private Piece[] CastleDeepcopy(byte kingId, byte castleId)
    {
        Piece[] result = new Piece[Pieces.Length];
        int i = 0;
        foreach (Piece p in Pieces)
        {
            // Remove this piece from list, add it back with new position
            // Remove captures piece if applicable
            if (p.Id == kingId || castleId == p.Id)
                continue;
            Piece deepCopy = new(p.Id, p.BasePiece, p.Color, p.Position, p.Movement, p.SpecialPieceType);
            result[i] = deepCopy;
            i++;
        }

        return result;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Board board)
            return false;

        if (board.Turn != Turn)
            return false;
        if (!board.CastleKingSide.SequenceEqual(CastleKingSide))
            return false;
        if (!board.CastleQueenSide.SequenceEqual(CastleQueenSide))
            return false;
        if (board.LastMove != LastMove)
            return false;

        HashSet<Piece> ownPieces = Pieces.ToHashSet();
        HashSet<Piece> otherPieces = board.Pieces.ToHashSet();
        if (!ownPieces.SetEquals(otherPieces))
            return false;

        return true;
    }

    public override string ToString()
    {
        if (LastMove is null)
            return "Root board";
        return LastMove.ToString();
    }
}
