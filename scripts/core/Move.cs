using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public struct Move(byte pieceId, Vector2Int from, Vector2Int to, Piece captured = null)
{
    public Vector2Int From = from, To = to;
    public byte Moving = pieceId;
    public Piece Captured = captured;

    private static readonly char[] files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

    public static bool operator ==(Move? a, Move? b)
    {
        if (a is null || b is null)
            return false;

        return a?.Moving == b?.Moving && a?.From == b?.From && a?.To == b?.To && a?.Captured == b?.Captured;
    }

    public static bool operator !=(Move? a, Move? b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return $"{files[From.X]}{From.Y + 1}{files[To.X]}{To.Y + 1}";
    }
}
