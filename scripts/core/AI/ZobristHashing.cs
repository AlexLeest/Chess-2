using Godot;
using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// Second real engine. Builds on the first one but uses (custom) Zobrist hashing to increase performance
/// </summary>
/// <param name="maxDepth">Max search depth</param>
public class ZobristHashing(int maxDepth) : IEngine
{
    // private Board[] lastPrincipalVariation = [];
    
    private TranspositionTable transpositionTable = new();
    
    // Debug counts
    // private int tTableFinds, tTableUses, tTableMismatch;
    
    public Board GenerateNextMove(Board board)
    {
        // tTableFinds = tTableUses = tTableMismatch = 0;
        
        // lastPrincipalVariation = [];
        // transpositionTable.Clear();
        Move lastKnownBestMove = new();
        
        for (int i = 1; i <= maxDepth; i++)
        {
            NegaMax(board, float.NegativeInfinity, float.PositiveInfinity, i, out Move bestMove);
            // lastPrincipalVariation = principalVariation;
            lastKnownBestMove = bestMove;
        }
        transpositionTable.Clear();
        GC.Collect();

        // GD.Print($"Hash matches: {tTableFinds}, entry uses: {tTableUses}, mismatches: {tTableMismatch}");
        return board.ApplyMove(lastKnownBestMove);
    }

    private float NegaMax(Board board, float alpha, float beta, int depth, out Move bestMove)
    {
        bestMove = new Move();
        
        // Look up board in transposition table
        uint zobristHash = board.GetZobristHash();
        if (transpositionTable.TryGetEntry(zobristHash, out Entry entry))
        {
            // tTableFinds++;
            // if (!board.Equals(entry.BestMoves[^1]))
            //     tTableMismatch++;
            // GD.Print("Transposition table match found!");
            // If TTable entry has a higher depth (meaning more ply's searched down left), use those results instead
            if (entry.Depth >= depth)
            {
                // tTableUses++;
                // GD.Print("TTable match used instead of recalculating that shit!");
                bestMove = entry.BestMove;
                return entry.Score;
            }
        }
        
        if (depth <= 0)
        {
            bestMove = new Move();
            return DetermineScore(board);
        }

        float max = float.NegativeInfinity;
        List<Move> nextMoves = board.GetMoves();
        
        SortByPrincipalVariation(board, nextMoves, depth);
        
        if (nextMoves.Count == 0)
        {
            bestMove = new Move();
            if (board.IsInCheck(board.ColorToMove))
                return float.MinValue;
            return 0;
        }
        
        // Board[] bestMoves = [];
        foreach (Move move in nextMoves)
        {
            Board nextBoard = board.ApplyMove(move);
            if (nextBoard is null)
                continue;
            float score = -NegaMax(nextBoard, -beta, -alpha, depth - 1, out Move possibleMove);
            
            if (score > max)
            {
                // bestMoves = new Board[possibleMoves.Length + 1];
                // possibleMoves.CopyTo(bestMoves, 0);
                // bestMoves[^1] = board;
                
                // principalVariation = bestMoves;
                bestMove = move;
                max = score;
                alpha = Math.Max(alpha, score);
            }
            if (score >= beta)
            {
                break;
            }
        }

        transpositionTable.AddEntry(zobristHash, depth, max, bestMove);
        
        // principalVariation = bestMoves;
        return max;
    }

    private void SortByPrincipalVariation(Board board, List<Move> moves, int depth)
    {
        // Check if this list of moves has the pre-calculated principal variation in there as an option
        // If so, put that up front
        // int pvIndex = depth - 2;
        // if (pvIndex < 0 || pvIndex >= lastPrincipalVariation.Length)
        //     return;
        //
        // Board moveToPrioritize = lastPrincipalVariation[pvIndex];
        if (!transpositionTable.TryGetEntry(board.ZobristHash, out Entry entry))
        {
            return;
        }
        Move moveToPrioritize = entry.BestMove;
        int index = moves.FindIndex(move => move == moveToPrioritize);
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
