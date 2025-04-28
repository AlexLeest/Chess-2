using System;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class DefaultMovements
{
    public static IMovement Get(BasePiece pieceType)
    {
        switch (pieceType)
        {
            case BasePiece.KING:
                return SlidingMovement.King;
            case BasePiece.QUEEN:
                return SlidingMovement.Queen;
            case BasePiece.BISHOP:
                return SlidingMovement.Bishop;
            case BasePiece.ROOK:
                return SlidingMovement.Rook;
            case BasePiece.KNIGHT:
                return SlidingMovement.Knight;
            case BasePiece.PAWN:
                return new PawnMovement();
        }
        throw new ArgumentException($"BasePiece type {pieceType} not defined");
    }
}
