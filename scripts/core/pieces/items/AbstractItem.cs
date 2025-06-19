using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

public abstract class AbstractItem(byte pieceId, ItemTriggers trigger) : IItem
{
    protected Dictionary<(bool, Vector2Int), int> hashValues = [];
    private Random rng = new();
    
    public byte PieceId { get; } = pieceId;
    public ItemTriggers Trigger { get; } = trigger;

    public virtual bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        return true;
    }

    public abstract Board Execute(Board board, Move move, IBoardEvent trigger);

    public virtual uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }
}
