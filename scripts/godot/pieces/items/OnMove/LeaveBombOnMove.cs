using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnMove;

public partial class LeaveBombOnMove : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        return "Leaves bomb behind on the spot the piece moved from.\nBomb sticks around 1 turn and kills any piece that tries to capture it.";
    }
}
