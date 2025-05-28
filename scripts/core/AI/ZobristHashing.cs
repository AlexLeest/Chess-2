using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// Second real engine. Builds on the first one but uses (custom) Zobrist hashing to increase performance
/// </summary>
/// <param name="maxDepth">Max search depth</param>
public class ZobristHashing(int maxDepth) : IEngine
{
    private Board[] lastPrincipalVariation = [];
    
    private TranspositionTable transpositionTable = new TranspositionTable();
    
    public Board GenerateNextMove(Board board)
    {
        lastPrincipalVariation = [];
        
        for (int i = 1; i <= maxDepth; i++)
        {
            NegaMax(board, float.NegativeInfinity, float.PositiveInfinity, i, out Board[] principalVariation);
            lastPrincipalVariation = principalVariation;
        }

        return lastPrincipalVariation[^2];
    }

    private float NegaMax(Board board, float alpha, float beta, int depth, out Board[] principalVariation)
    {
        // Look up board in transposition table
        int zobristHash = board.GetZobristHash();
        if (transpositionTable.TryGetEntry(zobristHash, out Entry entry))
        {
            // If TTable entry has a higher depth (meaning more ply's searched down left), use those results instead
            if (entry.Depth >= depth)
            {
                principalVariation = entry.BestMoves;
                return entry.Score;
            }
        }
        
        if (depth <= 0)
        {
            principalVariation = [board];
            return DetermineScore(board);
        }

        float max = float.NegativeInfinity;
        List<Board> nextMoves = board.GenerateMoves();
        
        SortByPrincipalVariation(nextMoves, depth);
        
        if (nextMoves.Count == 0)
        {
            principalVariation = [board];
            if (board.IsInCheck(board.ColorToMove))
                return float.MinValue;
            return 0;
        }
        
        Board[] bestMoves = [];
        foreach (Board move in nextMoves)
        {
            float score = -NegaMax(move, -beta, -alpha, depth - 1, out Board[] possibleMoves);
            
            if (score > max)
            {
                bestMoves = new Board[possibleMoves.Length + 1];
                possibleMoves.CopyTo(bestMoves, 0);
                bestMoves[^1] = board;
                
                principalVariation = bestMoves;
                max = score;
                alpha = Math.Max(alpha, score);
            }
            if (score >= beta)
            {
                break;
            }
        }

        transpositionTable.AddEntry(zobristHash, depth, max, bestMoves);
        
        principalVariation = bestMoves;
        return max;
    }

    private void SortByPrincipalVariation(List<Board> moves, int depth)
    {
        // Check if this list of moves has the pre-calculated principal variation in there as an option
        // If so, put that up front
        int pvIndex = depth - 2;
        if (pvIndex < 0 || pvIndex >= lastPrincipalVariation.Length)
            return;
        
        Board moveToPrioritize = lastPrincipalVariation[pvIndex];
        int index = moves.FindIndex(move => move.LastMove == moveToPrioritize.LastMove);
        if (index == -1)
            return;

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
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
