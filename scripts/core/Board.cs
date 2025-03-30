using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Board
{
    private Piece[] pieces;
    private Piece[,] board;

    public Board(Piece[] pieces)
    {
        this.pieces = pieces;
        board = new Piece[8, 8];
        foreach (Piece piece in pieces)
        {
            board[piece.Position.X, piece.Position.Y] = piece;
        }
    }

    private List<Board> GenerateMoves()
    {
        List<Board> result = [];
        foreach (Piece piece in pieces)
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

        return result;
    }
}
