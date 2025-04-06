using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Board
{
    public Piece[] Pieces;
    public Piece[,] Squares;

    private int turn;

    public Board(int turn, Piece[] pieces)
    {
        this.turn = turn;
        Pieces = pieces;
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

            Piece.Rook(8, true, new Vector2Int(0, 0)),
            Piece.Knight(9, true, new Vector2Int(1, 0)),
            Piece.Bishop(10, true, new Vector2Int(2, 0)),
            Piece.Queen(11, true, new Vector2Int(3, 0)),
            Piece.King(12, true, new Vector2Int(4, 0)),
            Piece.Bishop(13, true, new Vector2Int(5, 0)),
            Piece.Knight(14, true, new Vector2Int(6, 0)),
            Piece.Rook(15, true, new Vector2Int(7, 0)),

            // Black
            Piece.Pawn(16, false, new Vector2Int(0, 6)),
            Piece.Pawn(17, false, new Vector2Int(1, 6)),
            Piece.Pawn(18, false, new Vector2Int(2, 6)),
            Piece.Pawn(19, false, new Vector2Int(3, 6)),
            Piece.Pawn(20, false, new Vector2Int(4, 6)),
            Piece.Pawn(21, false, new Vector2Int(5, 6)),
            Piece.Pawn(22, false, new Vector2Int(6, 6)),
            Piece.Pawn(23, false, new Vector2Int(7, 6)),

            Piece.Rook(24, false, new Vector2Int(0, 7)),
            Piece.Knight(25, false, new Vector2Int(1, 7)),
            Piece.Bishop(26, false, new Vector2Int(2, 7)),
            Piece.Queen(27, false, new Vector2Int(3, 7)),
            Piece.King(28, false, new Vector2Int(4, 7)),
            Piece.Bishop(29, false, new Vector2Int(5, 7)),
            Piece.Knight(30, false, new Vector2Int(6, 7)),
            Piece.Rook(31, false, new Vector2Int(7, 7)),
        ];
        
        return new Board(0, pieces);
    }

    public bool IsInCheck(bool color)
    {
        Vector2Int kingPosition = new(0, 0);
        int kingId = color ? 12 : 28;
        foreach (Piece piece in Pieces)
        {
            if (piece.Id != kingId)
                continue;
            kingPosition = piece.Position;
            break;
        }
        
        foreach (Piece piece in Pieces)
            foreach (IMovement movement in piece.Movement)
                foreach (Vector2Int move in movement.GetMovementOptions(piece.Position, Squares, piece.Color))
                    if (move == kingPosition)
                        return true;

        return false;
    }

    public List<Board> GenerateMoves()
    {
        // TODO:
        //  En passant
        //  Castling
        //  Pawn promotion
        
        bool colorToMove = turn % 2 == 0;
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

    private List<Board> GenerateMoves(Piece piece)
    {
        bool colorToMove = turn % 2 == 0;
        int nextTurn = turn + 1;
        List<Board> result = [];
        
        foreach (IMovement movement in piece.Movement)
            foreach (Vector2Int move in movement.GetMovementOptions(piece.Position, Squares, piece.Color))
            {
                // Take list of pieces on this board, copy it
                Piece capturedPiece = Squares[move.X, move.Y];
                Piece[] newPieces = DeepcopyPieces(capturedPiece == null ? [piece] : [piece, capturedPiece]);
                // TODO: If piece has PawnMovement and is on last row, promote (how to handle promotion to 4 separate things?)
                Piece newPiece;
                if (movement is PawnMovement && move.Y == (piece.Color ? 7 : 0))
                {
                    // Promotion! Just to queen for now
                    newPiece = new Piece(piece.Id, piece.Color, piece.Position, [SlidingMovement.Queen]);
                }
                else
                {
                    newPiece = new Piece(piece.Id, piece.Color, move, piece.Movement);
                }
                newPieces[^1] = newPiece;
                // if (capturedPiece is not null)
                // {
                //     // TODO: Item triggers onCapture and onDeath
                // }
                
                // Make new board add to results

                Board possibleMove = new(nextTurn, newPieces);
                if (!possibleMove.IsInCheck(colorToMove))
                    result.Add(possibleMove);
            }

        return result;
    }

    private Piece[] DeepcopyPieces(params Piece[] toSkip)
    {
        // Result leaves 1 "empty" spot in the array (to be filled with the moved piece)
        Piece[] result = new Piece[Pieces.Length - toSkip.Length + 1];
        int i = 0;
        foreach (Piece p in Pieces)
        {
            // Remove this piece from list, add it back with new position
            // Remove captures piece if applicable
            if (!toSkip.Contains(p))
            {
                // TODO: Find alternative to massive amounts of deep copying
                Piece deepCopy = new(p.Id, p.Color, p.Position, p.Movement);
                result[i] = deepCopy;
                i++;
            }
        }

        return result;
    }
}
