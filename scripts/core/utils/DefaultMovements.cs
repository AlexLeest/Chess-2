using System;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class DefaultMovements
{
    public static IMovement[] Get(BasePiece pieceType)
    {
        return pieceType switch
        {
            BasePiece.KING => [SlidingMovement.King, new CastlingMovement()],
            BasePiece.QUEEN => [SlidingMovement.Queen],
            BasePiece.BISHOP => [SlidingMovement.Bishop],
            BasePiece.ROOK => [SlidingMovement.Rook],
            BasePiece.KNIGHT => [SlidingMovement.Knight],
            BasePiece.PAWN => [new PawnMovement()],
            BasePiece.CHECKERS => [new CheckersMovement()],
            _ => throw new ArgumentException($"BasePiece type {pieceType} not defined"),
        };
    }
}
