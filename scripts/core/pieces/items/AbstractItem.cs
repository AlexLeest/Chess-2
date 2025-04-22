using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public abstract class AbstractItem(byte pieceId, ItemTriggers trigger) : IItem
{
    public byte PieceId { get; } = pieceId;
    public ItemTriggers Trigger { get; } = trigger;

    public virtual bool ConditionsMet(Board board, Move move)
    {
        return false;
    }

    public virtual Board Execute(Board board, Move move)
    {
        throw new System.NotImplementedException();
    }
}
