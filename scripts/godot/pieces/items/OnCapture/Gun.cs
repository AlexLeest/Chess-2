using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

[GlobalClass]
public partial class Gun : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.Gun(pieceId);
    }
}
