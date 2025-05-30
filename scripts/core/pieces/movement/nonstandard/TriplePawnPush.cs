using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;
using System.Numerics;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement.nonstandard;

/// <summary>
/// Allows for a TRIPLE jump from a pawn, but only during the first turn (first 2 plies)
/// </summary>
public class TriplePawnPush : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        if (board.Turn > 2)
            return [];

        if (from.Y is not (1 or 6))
            return [];

        int offset = color ? 3 : -3;
        Vector2Int goalPos = new(from.X, from.Y + offset);
        if (board.Squares[goalPos.X, goalPos.Y] is not null)
            return [];

        return [new Move(id, from, goalPos)];
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        // None of these move capture anything
        return false;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        // None of these move capture anything
        return false;
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "TRIPLE PAWN PUSH";
    }
}
