using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnMove;

[GlobalClass]
public partial class GDLeaveBombOnMove : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new LeaveBombOnMove(pieceId);
    }

    public override string GetDescription()
    {
        return "Bomb Voyage:\nLeaves bomb behind on the spot the piece moved from.\nBomb sticks around 1 turn and kills any piece that tries to capture it.";
    }
}
