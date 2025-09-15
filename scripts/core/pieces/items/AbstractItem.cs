using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.AfterCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.BeforeCapture;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnOpponentCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnPromotion;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System;
using System.Collections.Generic;
using System.Drawing;

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

    public abstract IItem GetNewInstance(byte pieceId);

    public virtual uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }
}
