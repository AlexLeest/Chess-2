using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public abstract class AbstractItem(byte pieceId, ItemTriggers trigger) : IItem
{
    public byte PieceId { get; } = pieceId;
    public ItemTriggers Trigger { get; } = trigger;

    public virtual bool ConditionsMet(Piece[] pieces, Vector2Int position)
    {
        return false;
    }

    public virtual Piece[] Execute(Piece[] pieces, Vector2Int position)
    {
        throw new System.NotImplementedException();
    }
}
