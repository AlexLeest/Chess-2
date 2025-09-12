using Godot;
using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// First earnest attempt at a chess AI:
/// Use iterative negamax with alpha-beta pruning to a set depth (no quiescence search).
/// Then use naive piece scores (no positioning, no items being taken into account) to determine the heuristic score of the leaf boards.
///
/// DEVELOPMENT ON THIS ENGINE IS DONE, MAKE A NEW ONE
/// </summary>
public class MinimaxWithPieceHeuristic(int maxDepth) : IEngine
{
    // private Board bestMove;
    // private float bestEval;
    // private int betaPruned;
    // private int nodeCount;

    // private int sortAmount;
    private Move[] lastPrincipalVariation = [];
    
    public Move GenerateNextMove(Board board)
    {
        lastPrincipalVariation = [];
        // bestEval = 0;
        // betaPruned = 0;
        // nodeCount = 0;
        
        for (int i = 1; i <= maxDepth; i++)
        {
            // sortAmount = 0;
            // betaPruned = 0;
            // nodeCount = 0;
            NegaMax(board, float.NegativeInfinity, float.PositiveInfinity, i, out Move[] principalVariation);
            lastPrincipalVariation = principalVariation;
        }
        // GD.Print($"Pruned amount: {betaPruned}, sorted amount: {sortAmount}, nodes: {nodeCount}");

        return lastPrincipalVariation[^1];
    }

    private float NegaMax(Board board, float alpha, float beta, int depth, out Move[] principalVariation)
    {
        // nodeCount++;
        if (depth <= 0)
        {
            principalVariation = [];
            return DetermineScore(board);
        }

        float max = float.NegativeInfinity;
        List<Move> nextMoves = board.GetMoves();
        
        SortByPrincipalVariation(nextMoves, depth);
        
        if (nextMoves.Count == 0)
        {
            principalVariation = [];
            if (board.IsInCheck(board.ColorToMove))
                return float.MinValue;
            return 0;
        }
        
        Move[] bestMoves = [nextMoves[0]];
        foreach (Move move in nextMoves)
        {
            Board nextBoard = move.Result;
            if (nextBoard is null)
                continue;
            
            float score = -NegaMax(nextBoard, -beta, -alpha, depth - 1, out Move[] possibleMoves);
            
            if (score > max)
            {
                bestMoves = new Move[possibleMoves.Length + 1];
                possibleMoves.CopyTo(bestMoves, 0);
                bestMoves[^1] = move;
                
                principalVariation = bestMoves;
                max = score;
                alpha = Math.Max(alpha, score);
            }
            if (score >= beta)
            {
                principalVariation = bestMoves;
                // betaPruned++;
                return max;
            }
        }

        principalVariation = bestMoves;
        return max;
    }

    private void SortByPrincipalVariation(List<Move> moves, int depth)
    {
        // Check if this list of moves has the pre-calculated principal variation in there as an option
        // If so, put that up front
        int pvIndex = depth - 2;
        if (pvIndex < 0 || pvIndex >= lastPrincipalVariation.Length)
            return;
        
        Move moveToPrioritize = lastPrincipalVariation[pvIndex];
        int index = moves.FindIndex(move => move == moveToPrioritize);
        if (index == -1)
            return;

        // sortAmount++;
        (moves[index], moves[0]) = (moves[0], moves[index]);
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
            BasePiece.BOMB => 1f * sign,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
