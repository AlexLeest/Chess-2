using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

public interface IItem
{
    public byte PieceId { get; }
    public ItemTriggers Trigger { get; }
    
    public bool ConditionsMet(Board board, Move move);

    public Board Execute(Board board, Move move, ref List<IBoardEvent> events);

    public uint GetZobristHash(bool color, Vector2Int position);
}
