using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

[GlobalClass]
public partial class SelfDestruct : GodotItem
{
    public override ItemRarity Rarity => ItemRarity.COMMON;

    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCaptured.SelfDestruct(pieceId);
    }

    public override string GetDescription()
    {
        return "Self Destruct\nThis piece kills any piece that captures it.";
    }
}
