using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public class SelfDestruct(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURED)
{
    public override bool ConditionsMet(Piece[] pieces, Vector2Int position)
    {
        return true;
    }

    public override Piece[] Execute(Piece[] pieces, Vector2Int position)
    {
        return base.Execute(pieces, position);
    }
}
