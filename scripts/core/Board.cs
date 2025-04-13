using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Board
{
    public readonly Piece[] Pieces;
    public readonly Piece[,] Squares;
    public readonly int Turn;

    private readonly bool[] castleQueenSide;
    private readonly bool[] castleKingSide;
    private readonly bool enPassantPossible;

    public Board(
        int turn,
        Piece[] pieces,
        bool[] castleQueenSide,
        bool[] castleKingSide,
        bool enPassantPossible = false
    )
    {
        Turn = turn;
        Pieces = pieces;
        Squares = new Piece[8, 8];
        foreach (Piece piece in pieces)
        {
            Squares[piece.Position.X, piece.Position.Y] = piece;
        }

        this.castleQueenSide = castleQueenSide;
        this.castleKingSide = castleKingSide;
        this.enPassantPossible = enPassantPossible;
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

        return new Board(0, pieces, [true, true], [true, true]);
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

    private bool IsInCheck(bool color, Vector2Int position)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece.Color == color)
                continue;
            foreach (Vector2Int move in piece.GetMovementOptions(Squares))
                if (move == position)
                    return true;
        }

        return false;
    }

    private bool IsInCheck(bool color, Vector2Int[] positions)
    {
        foreach (Piece piece in Pieces)
        {
            if (piece.Color == color)
                continue;
            foreach (Vector2Int move in piece.GetMovementOptions(Squares))
                if (positions.Contains(move))
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
        foreach (Vector2Int move in piece.GetMovementOptions(Squares))
        {
            Piece capturedPiece = Squares[move.X, move.Y];

            // Check en passant captures
            bool enPassantMove = false;
            if (enPassantPossible && piece.SpecialPieceType == SpecialPieceTypes.PAWN && capturedPiece == null)
            {
                Piece enPassantCheck = Squares[move.X, piece.Position.Y];
                if (enPassantCheck != null && enPassantCheck.Color != colorToMove && enPassantCheck.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
                    capturedPiece = enPassantCheck;
            }

            // Make full copy of all unmoved pieces
            Piece[] newPieces;
            if (capturedPiece == null)
                newPieces = DeepcopyPieces(piece.Id);
            else
                newPieces = DeepcopyPieces(piece.Id, capturedPiece.Id);

            // Add moved piece (in new position) back to the list
            Piece newPiece;
            if (piece.SpecialPieceType == SpecialPieceTypes.PAWN && move.Y == (piece.Color ? 7 : 0))
            {
                // Promotion! Just to queen for now
                // TODO: Promotion to bishop, rook, knight
                newPiece = new Piece(piece.Id, piece.Color, move, [SlidingMovement.Queen]);
            }
            else if (piece.SpecialPieceType == SpecialPieceTypes.PAWN && Math.Abs(move.Y - piece.Position.Y) == 2)
            {
                newPiece = new Piece(piece.Id, piece.Color, move, piece.Movement, SpecialPieceTypes.EN_PASSANTABLE_PAWN);
                enPassantMove = true;
            }
            else
            {
                newPiece = new Piece(piece.Id, piece.Color, move, piece.Movement, piece.SpecialPieceType);
            }
            newPieces[^1] = newPiece;
            // if (capturedPiece is not null)
            // {
            //     // TODO: Item triggers onCapture and onDeath
            // }

            // If any of the castling pieces move, disallow castling in future boards
            bool[] newCastleQueenSide = [castleQueenSide[0], castleQueenSide[1]];
            bool[] newCastleKingSide = [castleKingSide[0], castleKingSide[1]];
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
            if (capturedPiece != null)
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
            Board possibleMove = new(nextTurn, newPieces, newCastleQueenSide, newCastleKingSide, enPassantMove);
            if (!possibleMove.IsInCheck(colorToMove))
                result.Add(possibleMove);
        }

        // Castling, separate from any preset movements
        if (piece.SpecialPieceType == SpecialPieceTypes.KING && !IsInCheck(colorToMove))
        {
            int colorRank = colorToMove ? 0 : 7;
            if (castleKingSide[colorIndex])
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
                    Piece[] newPieces = CastleDeepcopy(piece.Id, toCastle.Id);
                    // Move king
                    newPieces[^2] = new Piece(piece.Id, piece.Color, new Vector2Int(6, colorRank), piece.Movement, piece.SpecialPieceType);
                    // Move piece
                    newPieces[^1] = new Piece(toCastle.Id, toCastle.Color, new Vector2Int(5, colorRank), piece.Movement, piece.SpecialPieceType);
                    
                    // Remove castling rights
                    bool[] newCastleQueenSide = [castleQueenSide[0], castleQueenSide[1]];
                    bool[] newCastleKingSide = [castleKingSide[0], castleKingSide[1]];
                    newCastleQueenSide[colorIndex] = false;
                    newCastleKingSide[colorIndex] = false;
                    Board castledBoard = new(nextTurn, newPieces, newCastleQueenSide, newCastleKingSide);
                    
                    // Add to results
                    result.Add(castledBoard);
                }
            }
            if (castleQueenSide[colorIndex])
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
                    Piece[] newPieces = CastleDeepcopy(piece.Id, toCastle.Id);
                    // Move king
                    newPieces[^2] = new Piece(piece.Id, piece.Color, new Vector2Int(2, colorRank), piece.Movement, piece.SpecialPieceType);
                    // Move piece
                    newPieces[^1] = new Piece(toCastle.Id, toCastle.Color, new Vector2Int(3, colorRank), piece.Movement, piece.SpecialPieceType);
                    
                    // Remove castling rights
                    bool[] newCastleQueenSide = [castleQueenSide[0], castleQueenSide[1]];
                    bool[] newCastleKingSide = [castleKingSide[0], castleKingSide[1]];
                    newCastleQueenSide[colorIndex] = false;
                    newCastleKingSide[colorIndex] = false;
                    Board castledBoard = new(nextTurn, newPieces, newCastleQueenSide, newCastleKingSide);
                    
                    // Add to results
                    result.Add(castledBoard);
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
            Piece deepCopy = new(p.Id, p.Color, p.Position, p.Movement, p.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN ? SpecialPieceTypes.PAWN : p.SpecialPieceType);
            result[i] = deepCopy;
            i++;
        }

        return result;
    }

    public Piece[] CastleDeepcopy(byte kingId, byte castleId)
    {
        // Result leaves 1 "empty" spot in the array (to be filled with the moved piece)
        Piece[] result = new Piece[Pieces.Length];
        int i = 0;
        foreach (Piece p in Pieces)
        {
            // Remove this piece from list, add it back with new position
            // Remove captures piece if applicable
            if (p.Id == kingId || castleId == p.Id)
                continue;
            Piece deepCopy = new(p.Id, p.Color, p.Position, p.Movement, p.SpecialPieceType);
            result[i] = deepCopy;
            i++;
        }

        return result;
    }
}
