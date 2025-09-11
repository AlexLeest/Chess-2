using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
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
        if (board.Turn > 2 || from.Y is not (1 or 6))
            return [];
        
        int offset = color ? 1 : -1;
        int x = from.X;
        int y = from.Y;
        Vector2Int goalPos = new(x, y + (offset * 3));
        if (board.Squares[x, y + offset] != null || board.Squares[x, y + (offset * 2)] != null || board.Squares[goalPos.X, goalPos.Y] != null)
            return [];

        Move move = new(id, from, goalPos, board);
        move.ApplyEvent(new MovePieceEvent(id, from, goalPos));
        move.ApplyEvent(new NextTurnEvent());

        return [move];
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
