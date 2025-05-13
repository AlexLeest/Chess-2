using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// First earnest attempt at a chess AI:
/// Use iterative deepening negamax with alpha-beta pruning to a set depth (no quiescence search).
/// Then use naive piece scores (no positioning, no items being taken into account) to determine the heuristic score of the leaf boards.
/// </summary>
public class MinimaxWithPieceHeuristic(int maxDepth) : IEngine
{
    private int maxDepth = maxDepth;

    private Board bestMove;
    private float bestEval;
    private int betaPruned;
    
    public Board GenerateNextMove(Board board)
    {
        Stopwatch timer = Stopwatch.StartNew();
        betaPruned = 0;
        float score = NegaMax(board, float.NegativeInfinity, float.PositiveInfinity, maxDepth);
        GD.Print($"{bestEval}/{score}, {bestMove}, {timer.Elapsed}, pruned: {betaPruned}");
        return bestMove;
    }

    private float NegaMax(Board board, float alpha, float beta, int depth)
    {
        if (depth == 0)
        {
            return DetermineScore(board);
        }

        float max = float.NegativeInfinity;
        List<Board> nextMoves = board.GenerateMoves();
        if (nextMoves.Count == 0)
        {
            if (board.IsInCheck(board.ColorToMove))
                return float.NegativeInfinity;
            return 0;
        }
        
        foreach (Board move in nextMoves)
        {
            float score = -NegaMax(move, -beta, -alpha, depth - 1);
            if (score > max)
            {
                max = score;
                alpha = Math.Max(alpha, score);

                if (depth == maxDepth)
                {
                    bestMove = move;
                    bestEval = score;
                }
            }
            if (score >= beta)
            {
                betaPruned++;
                return max;
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
        
        return board.ColorToMove ? score : -score;
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
