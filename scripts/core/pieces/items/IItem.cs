using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public interface IItem
{
    public byte PieceId { get; }
    public ItemTriggers Trigger { get; }
    
    public bool ConditionsMet(Board board, Vector2Int position);

    public Board Execute(Board board, Vector2Int position);
}
