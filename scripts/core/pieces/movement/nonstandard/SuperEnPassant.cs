using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement.nonstandard;

/// <summary>
/// Movement that allows en-passant like capture of anything left or right of this piece. TODO: Untested
/// </summary>
public class SuperEnPassant : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        List<Move> options = [];
        Vector2Int forward = color ? Vector2Int.Up : Vector2Int.Down;
        
        Vector2Int left = from + Vector2Int.Left;
        AttemptMove(id, from, board, color, left, forward, options);
        
        Vector2Int right = from + Vector2Int.Left;
        AttemptMove(id, from, board, color, right, forward, options);
        
        return options;
    }

    private static void AttemptMove(byte id, Vector2Int from, Board board, bool color, Vector2Int capturePos, Vector2Int forward, List<Move> options)
    {
        Vector2Int goalPos = capturePos + forward;
        Piece toCapture = board.Squares.Get(capturePos);
        
        if (toCapture is null || toCapture.Color == color || board.Squares.Get(goalPos) is not null)
            return;
        
        Move move = new(id, from, goalPos, board);
        move.ApplyEvent(new MovePieceEvent(id, from, goalPos));
        move.ApplyEvent(new CapturePieceEvent(toCapture.Id, id));

        options.Add(move);
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        if (target.Y != from.Y)
            return false;
        
        int deltaX = target.X - from.X;
        if (deltaX != 1 && deltaX != -1)
            return false;
        
        Vector2Int forward = color ? Vector2Int.Up : Vector2Int.Down;
        Vector2Int goalPos = target + forward;
        return board.Squares.Get(goalPos) is null;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        Vector2Int forward = color ? Vector2Int.Up : Vector2Int.Down;
        foreach (Vector2Int target in targets)
        {
            if (target.Y != from.Y)
                continue;
        
            int deltaX = target.X - from.X;
            if (deltaX != 1 && deltaX != -1)
                continue;
        
            Vector2Int goalPos = target + forward;
            if (board.Squares.Get(goalPos) is null)
                return true;
        }
        return false;
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "SUPER EN PASSANT";
    }
}
