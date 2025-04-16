using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public struct Move(Vector2Int from, Vector2Int to)
{
    public Vector2Int From = from, To = to;

    private static readonly char[] files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

    public override string ToString()
    {
        return $"{files[From.X]}{From.Y + 1}{files[To.X]}{To.Y + 1}";
    }
}
