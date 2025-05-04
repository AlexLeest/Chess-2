namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

public interface IItem
{
    public byte PieceId { get; }
    public ItemTriggers Trigger { get; }
    
    public bool ConditionsMet(Board board, Move move);

    public Board Execute(Board board, Move move);
}
