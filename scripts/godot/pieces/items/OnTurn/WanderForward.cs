using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items.OnTurn;

[GlobalClass]
public partial class WanderForward : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.buffs.OnTurn.WanderForward(pieceId);
    }
}
