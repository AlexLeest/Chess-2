using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public struct Move(byte pieceId, Vector2Int from, Vector2Int to, IBoardEvent[] events, Board result)
{
    // id of the piece that got the move command
    public byte Moving = pieceId;
    // positions the piece was moved from/to.
    // To does not necessarily match up with the resulting position!
    public Vector2Int From = from, To = to;
    // List of events that happened because of this move, in order.
    public IBoardEvent[] Events = events;
    // Resulting board state (can be an illegal board)
    public Board Result = result;
    
    // public Piece Captured = captured;
    // public SpecialMoveFlag Flag = flag;

    public static bool operator ==(Move? a, Move? b)
    {
        if (a is null || b is null)
            return false;

        // return a?.Moving == b?.Moving && a?.From == b?.From && a?.To == b?.To && a?.Captured == b?.Captured;
        return a?.Moving == b?.Moving && a?.From == b?.From && a?.To == b?.To;
    }

    public static bool operator !=(Move? a, Move? b)
    {
        return !(a == b);
    }

    private static readonly char[] files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

    public override string ToString()
    {
        return $"{files[From.X]}{From.Y + 1}{files[To.X]}{To.Y + 1}";
    }
}

public enum SpecialMoveFlag : byte
{
    NONE,
    CASTLE_KINGSIDE,
    CASTLE_QUEENSIDE,
    PROMOTION,
}