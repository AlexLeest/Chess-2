using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Piece
{
    public bool Color;
    public Vector2Int Position;
    public IMovement Movement;

    public Piece(bool color, Vector2Int position, IMovement movement)
    {
        Color = color;
        Position = position;
        Movement = movement;
    }

    public static Piece Pawn(bool color, Vector2Int position)
    {
        return new Piece(color, position, new PawnMovement());
    }

    public static Piece Knight(bool color, Vector2Int position)
    {
        return new Piece(color, position, SlidingMovement.Knight);
    }

    public static Piece Bishop(bool color, Vector2Int position)
    {
        return new Piece(color, position, SlidingMovement.Bishop);
    }

    public static Piece Rook(bool color, Vector2Int position)
    {
        return new Piece(color, position, SlidingMovement.Rook);
    }

    public static Piece Queen(bool color, Vector2Int position)
    {
        return new Piece(color, position, SlidingMovement.Queen);
    }

    public static Piece King(bool color, Vector2Int position)
    {
        return new Piece(color, position, SlidingMovement.King);
    }
}
