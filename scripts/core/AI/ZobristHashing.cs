using System;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// Second real engine. Builds on the first one but uses (custom) Zobrist hashing to increase performance
/// </summary>
/// <param name="maxDepth">Max search depth</param>
public class ZobristHashing(int maxDepth) : IEngine
{
    // TODO: Implement custom Zobrist hashing for pieces/moments/items
    public Board GenerateNextMove(Board board)
    {
        throw new System.NotImplementedException();
    }
    
    // Straight copy from MinimaxWithPieceHeuristic.cs
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
