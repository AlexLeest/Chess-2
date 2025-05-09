using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// First earnest attempt at a chess AI:
/// Use iterative deepening negamax with alpha-beta pruning to a set depth (no quiescence search).
/// Then use naive piece scores (no positioning, no items being taken into account) to determine the heuristic score of the leaf boards.
/// </summary>
public class MinimaxWithPieceHeuristic(int maxDepth) : IEngine
{
    private int maxDepth = maxDepth;
    
    public Board GenerateNextMove(Board board)
    {
        for (int depth = 0; depth < +maxDepth; depth++)
        {
            
        }
        
        return board;
    }

    private float NegaMax(Board board, int depth)
    {
        if (depth == 0)
            return DetermineScore(board);

        float max = float.NegativeInfinity;
        List<Board> nextMoves = board.GenerateMoves();
        foreach (Board move in nextMoves)
        {
            float score = -NegaMax(move, depth - 1);
            if (score > max)
            {
                max = score;
            }
        }
        return max;
    }

    public float DetermineScore(Board board)
    {
        float score = 0;
        
        foreach (Piece piece in board.Pieces)
        {
            score += ScoreForPiece(piece);
        }

        return score;
    }

    private float ScoreForPiece(Piece piece)
    {
        int sign = piece.Color ? 1 : -1;
        return piece.BasePiece switch
        {
            BasePiece.PAWN => 1f * sign,
            BasePiece.KNIGHT => 3f * sign,
            BasePiece.BISHOP => 3f * sign,
            BasePiece.ROOK => 5f * sign,
            BasePiece.QUEEN => 9f * sign,
            BasePiece.KING => 0f * sign,
            BasePiece.CHECKERS => 1f * sign,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
