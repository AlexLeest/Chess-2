namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

public abstract class AbstractItem(byte pieceId, ItemTriggers trigger) : IItem
{
    public byte PieceId { get; } = pieceId;
    public ItemTriggers Trigger { get; } = trigger;

    public virtual bool ConditionsMet(Board board, Move move)
    {
        return true;
    }

    public abstract Board Execute(Board board, Move move);
}
