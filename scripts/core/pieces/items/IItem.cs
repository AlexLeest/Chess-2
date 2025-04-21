using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public interface IItem
{
    public byte PieceId { get; }
    public ItemTriggers Trigger { get; }
    
    public bool ConditionsMet(Piece[] pieces, Vector2Int position);

    public Piece[] Execute(Piece[] pieces, Vector2Int position);
}
