using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Board
{
    public Piece[] Pieces;
    
    private Piece[,] squares;

    public Board(Piece[] pieces)
    {
        this.Pieces = pieces;
        squares = new Piece[8, 8];
        foreach (Piece piece in pieces)
        {
            squares[piece.Position.X, piece.Position.Y] = piece;
        }
    }

    public static Board DefaultBoard()
    {
        Piece[] pieces = [
            // White
            Piece.Pawn(true, new Vector2Int(0,1)),
            Piece.Pawn(true, new Vector2Int(1,1)),
            Piece.Pawn(true, new Vector2Int(2,1)),
            Piece.Pawn(true, new Vector2Int(3,1)),
            Piece.Pawn(true, new Vector2Int(4,1)),
            Piece.Pawn(true, new Vector2Int(5,1)),
            Piece.Pawn(true, new Vector2Int(6,1)),
            Piece.Pawn(true, new Vector2Int(7,1)),
            
            Piece.Rook(true, new Vector2Int(0,0)),
            Piece.Knight(true, new Vector2Int(1,0)),
            Piece.Bishop(true, new Vector2Int(2,0)),
            Piece.King(true, new Vector2Int(3,0)),
            Piece.Queen(true, new Vector2Int(4,0)),
            Piece.Bishop(true, new Vector2Int(5,0)),
            Piece.Knight(true, new Vector2Int(6,0)),
            Piece.Rook(true, new Vector2Int(7,0)),
            
            // Black
            Piece.Pawn(false, new Vector2Int(0,6)),
            Piece.Pawn(false, new Vector2Int(1,6)),
            Piece.Pawn(false, new Vector2Int(2,6)),
            Piece.Pawn(false, new Vector2Int(3,6)),
            Piece.Pawn(false, new Vector2Int(4,6)),
            Piece.Pawn(false, new Vector2Int(5,6)),
            Piece.Pawn(false, new Vector2Int(6,6)),
            Piece.Pawn(false, new Vector2Int(7,6)),
            
            Piece.Rook(false, new Vector2Int(0,6)),
            Piece.Knight(false, new Vector2Int(1,6)),
            Piece.Bishop(false, new Vector2Int(2,6)),
            Piece.King(false, new Vector2Int(3,6)),
            Piece.Queen(false, new Vector2Int(4,6)),
            Piece.Bishop(false, new Vector2Int(5,6)),
            Piece.Knight(false, new Vector2Int(6,6)),
            Piece.Rook(false, new Vector2Int(7,6)),
        ];
        
        return new Board(pieces);
    }

    private List<Board> GenerateMoves()
    {
        List<Board> result = [];
        foreach (Piece piece in Pieces)
        {
            foreach (Board move in GenerateMoves(piece))
            {
                result.Add(move);
            }
        }
        return result;
    }

    private List<Board> GenerateMoves(Piece piece)
    {
        List<Board> result = [];
        
        foreach (Vector2Int move in piece.Movement.GetMovementOptions(piece.Position, squares, piece.Color))
        {
            // Take list of pieces on this board, copy it
            Piece capturedPiece = squares[move.X, move.Y];
            List<Piece> piecesWithoutSubject = [];
            foreach (Piece p in Pieces)
            {
                // Remove this piece from list, add it back with new position
                // Remove captures piece if applicable
                if (p != piece && p != capturedPiece)
                {
                    // TODO: Find alternative to massive amounts of deep copying
                    Piece deepCopy = new(p.Color, p.Position, p.Movement);
                    piecesWithoutSubject.Add(deepCopy);
                }
            }
            // if (capturedPiece is not null)
            // {
            //     // TODO: Item trigger
            // }
            
            // Make new board add to results
            Board possibleMove = new(piecesWithoutSubject.ToArray());
            result.Add(possibleMove);
        }

        return result;
    }
}
