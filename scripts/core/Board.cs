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

    private bool whiteCastleQueenSide, whiteCastleKingSide, blackCastleQueenSide, blackCastleKingSide;

    public Board(int turn, Piece[] pieces, bool whiteCastleQueenSide = true, bool whiteCastleKingSide = true, bool blackCastleQueenSide = true, bool blackCastleKingSide = true)
    {
        Turn = turn;
        Pieces = pieces;
        Squares = new Piece[8, 8];
        foreach (Piece piece in pieces)
        {
            Squares[piece.Position.X, piece.Position.Y] = piece;
        }
        
        this.whiteCastleKingSide = whiteCastleKingSide;
        this.whiteCastleQueenSide = whiteCastleQueenSide;
        this.blackCastleKingSide = blackCastleKingSide;
        this.blackCastleQueenSide = blackCastleQueenSide;
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
        
        return new Board(0, pieces);
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
        
        foreach (Piece piece in Pieces)
            foreach (Vector2Int move in piece.GetMovementOptions(Squares))
                if (move == kingPosition)
                    return true;

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
        // TODO:
        //  En passant
        //  Castling
        //  Pawn promotion
        bool colorToMove = Turn % 2 == 0;
        int nextTurn = Turn + 1;
        List<Board> result = [];

        foreach (Vector2Int move in piece.GetMovementOptions(Squares))
        {
            // Take list of pieces on this board, copy it
            Piece capturedPiece = Squares[move.X, move.Y];
            Piece[] newPieces;
            if (capturedPiece == null)
                newPieces = DeepcopyPieces(piece.Id);
            else
                newPieces = DeepcopyPieces(piece.Id, capturedPiece.Id);
            // TODO: If piece has PawnMovement and is on last row, promote (how to handle promotion to 4 separate things?)
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

            // Make new board add to results
            Board possibleMove = new(nextTurn, newPieces, whiteCastleQueenSide, whiteCastleKingSide, blackCastleQueenSide, blackCastleKingSide);
            if (!possibleMove.IsInCheck(colorToMove))
                result.Add(possibleMove);
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
            // TODO: Find alternative to massive amounts of deep copying
            Piece deepCopy = new(p.Id, p.Color, p.Position, p.Movement, p.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN ? SpecialPieceTypes.PAWN : p.SpecialPieceType);
            result[i] = deepCopy;
            i++;
        }

        return result;
    }
}
