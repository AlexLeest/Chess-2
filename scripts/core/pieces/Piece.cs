using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Piece
{
    public byte Id;
    
    public bool Color;
    public Vector2Int Position;
    public IMovement[] Movement;

    public Piece(byte id, bool color, Vector2Int position, IMovement[] movement)
    {
        Id = id;
        Color = color;
        Position = position;
        Movement = movement;
    }

    public static Piece Pawn(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, color, position, [new PawnMovement()]);
    }

    public static Piece Knight(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, color, position, [SlidingMovement.Knight]);
    }

    public static Piece Bishop(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, color, position, [SlidingMovement.Bishop]);
    }

    public static Piece Rook(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, color, position, [SlidingMovement.Rook]);
    }

    public static Piece Queen(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, color, position, [SlidingMovement.Queen]);
    }

    public static Piece King(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, color, position, [SlidingMovement.King]);
    }
}
