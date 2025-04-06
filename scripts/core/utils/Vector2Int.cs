using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public struct Vector2Int
{
    public int X, Y;

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(a.X * b, a.Y * b);
    }

    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !(a == b);
    }

    public static bool operator ==(Vector2Int a, Vector2I b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2Int a, Vector2I b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public static Vector2Int Up => new(0, 1);
    public static Vector2Int Down => new(0, -1);
    public static Vector2Int Left => new(-1, 0);
    public static Vector2Int Right => new(1, 0);
    
    public static Vector2Int UpLeft => new(-1, 1);
    public static Vector2Int UpRight => new(1, 1);
    public static Vector2Int DownLeft => new(-1, -1);
    public static Vector2Int DownRight => new(1, -1);

    public static Vector2Int[] Cardinals => [Up, Down, Left, Right];
    public static Vector2Int[] Diagonals => [UpLeft, UpRight, DownLeft, DownRight];
    
    public static Vector2Int[] AllDirections => [Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight];

    public static Vector2Int[] KnightHops => [new(1, 2), new(2, 1), new(2, -1), new(1, -2), new(-1, -2), new(-2, -1), new(-2, 1), new(-1, 2)];
}

public static class Vector2IntExtensions
{
    public static bool Inside(this Vector2Int a, int width, int height)
    {
        return a.X >= 0 && a.Y >= 0 && a.X < width && a.Y < height;
    }
}
