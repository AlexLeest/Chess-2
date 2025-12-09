using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// Second real engine. Builds on the first one but uses (custom) Zobrist hashing to increase performance
/// </summary>
/// <param name="maxDepth">Max search depth</param>
public class ZobristWithQSearch(int maxDepth) : IEngine
{
    private TranspositionTable transpositionTable = new();
    
    // Debug counts
    private int tTableFinds, tTableUses, tTableMismatch;
    
    public Move GenerateNextMove(Board board)
    {
        tTableUses = 0;
        
        Move lastKnownBestMove = new();
        
        for (int i = 1; i <= maxDepth; i++)
        {
            NegaMax(board, float.NegativeInfinity, float.PositiveInfinity, i, out Move bestMove);
            lastKnownBestMove = bestMove;
        }
        transpositionTable.Clear();

        // GD.Print($"TTable used {tTableUses} times");
        return lastKnownBestMove;
    }

    private float NegaMax(Board board, float alpha, float beta, int depth, out Move bestMove)
    {
        bestMove = new Move();
        
        // Look up board in transposition table
        uint zobristHash = board.GetZobristHash();
        if (transpositionTable.TryGetEntry(zobristHash, out Entry entry))
        {
            // If TTable entry has a higher depth (meaning more ply's searched down left), use those results instead
            if (entry.Depth >= depth)
            {
                tTableUses++;
                // GD.Print("TTable match used instead of recalculating that shit!");
                bestMove = entry.BestMove;
                return entry.Score;
            }
        }
        
        if (depth <= 0)
        {
            // TODO: Add QSearch for non-quiet positions
            bestMove = new Move();
            return DetermineScore(board);
        }

        float max = float.NegativeInfinity;
        List<Move> nextMoves = board.GetMoves();
        
        SortByPrincipalVariation(board, nextMoves);
        
        if (nextMoves.Count == 0)
        {
            bestMove = new Move();
            // TODO: Replace with Move.IsLegal
            if (board.IsInCheck(board.ColorToMove))
                return float.MinValue;
            return 0;
        }
        
        foreach (Move move in nextMoves)
        {
            Board nextBoard = move.Result;
            if (nextBoard is null)
                continue;
            float score = -NegaMax(nextBoard, -beta, -alpha, depth - 1, out Move possibleMove);
            
            if (score > max)
            {
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

    private void SortByPrincipalVariation(Board board, List<Move> moves)
    {
        // Check if this list of moves has the pre-calculated principal variation in there as an option
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

    private bool PositionIsQuiet(List<Move> moves)
    {
        foreach (Move move in moves)
            foreach (IBoardEvent ev in move.Events)
                if (ev is CapturePieceEvent)
                    return false;

        return true;
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
