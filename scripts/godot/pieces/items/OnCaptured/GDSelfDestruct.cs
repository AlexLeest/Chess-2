using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCaptured;

[GlobalClass]
public partial class GDSelfDestruct : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCaptured.SelfDestruct(pieceId);
    }

    public override string GetDescription()
    {
        return "Self Destruct\nThis piece kills any piece that captures it.";
    }
}
