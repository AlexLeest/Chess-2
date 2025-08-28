using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;
using System.Reflection;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement;

/// <summary>
/// Can capture pieces of the same color at 1 distance.
/// </summary>
public class CannibalKing : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        List<Move> moves = [];
        
        for (int x=-1;x<=1;x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector2Int targetCoords = new(from.X + x, from.Y + y);
                if (!targetCoords.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
                    continue;
                
                Piece target = board.Squares[targetCoords.X, targetCoords.Y];
                if (target is null || target.Color != color || target.SpecialPieceType == SpecialPieceTypes.KING)
                    continue;

                Move move = new(id, from, targetCoords, board);
                move.ApplyEvent(new MovePieceEvent(id, targetCoords));
                move.ApplyEvent(new CapturePieceEvent(target.Id, id));

                moves.Add(move);
            }

        return moves;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        int deltaX = target.X - from.X;
        int deltaY = target.Y - from.Y;

        return deltaX is >= -1 and <= 1 && deltaY is >= -1 and <= 1;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        foreach (Vector2Int target in targets)
            if (Attacks(from, target, board, color))
                return true;
        return false;
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "CANNIBAL KING";
    }
}
