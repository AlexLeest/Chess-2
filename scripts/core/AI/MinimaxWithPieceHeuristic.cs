using Godot;
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
        List<Board> moves = board.GenerateMoves();
        Board bestMove = null;
        float bestMoveScore = float.NegativeInfinity;
        
        foreach (Board move in moves)
        {
            float moveScore = Mini(move, maxDepth);
            // GD.Print($"Move: {move.LastMove}, score: {moveScore}");
            if (moveScore > bestMoveScore)
            {
                // GD.Print($"Preferred move: {move.LastMove}");
                bestMoveScore = moveScore;
                bestMove = move;
            }
        }
        
        return bestMove;
    }

    private float Maxi(Board board, int depth)
    {
        if (depth == 0)
            return DetermineScore(board);

        float max = float.NegativeInfinity;
        List<Board> nextMoves = board.GenerateMoves();
        foreach (Board move in nextMoves)
        {
            float score = -Mini(move, depth - 1);
            if (score > max)
            {
                max = score;
            }
        }
        return max;
    }

    private float Mini(Board board, int depth)
    {
        if (depth == 0)
            return -DetermineScore(board);

        float min = float.PositiveInfinity;
        List<Board> nextMoves = board.GenerateMoves();
        foreach (Board move in nextMoves)
        {
            float score = -Maxi(move, depth - 1);
            if (score < min)
            {
                min = score;
            }
        }
        return min;
    }

    private float NegaMax(Board board, int depth)
    {
        if (depth == 0)
        {
            bool colorToMove = board.Turn % 2 == 0;
            return colorToMove ? -DetermineScore(board) : DetermineScore(board);
        }

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
        
        bool colorToMove = board.Turn % 2 == 0;
        return colorToMove ? -score : score;
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
