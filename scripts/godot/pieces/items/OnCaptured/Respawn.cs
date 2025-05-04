using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

[GlobalClass]
public partial class Respawn : GodotItem
{
    public override ItemRarity Rarity => ItemRarity.COMMON;

    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCaptured.Respawn(pieceId);
    }

    public override string GetDescription()
    {
        return "Respawn:\nThis piece will respawn (once) on the place where it started (if nothing else is standing there).";
    }
}
