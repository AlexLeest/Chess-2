using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCapture;

[GlobalClass]
public partial class GDCannibalTeeth : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCapture.CannibalTeeth(pieceId);
    }

    public override string GetDescription()
    {
        return "Cannibal teeth:\nWhen this piece captures a friendly piece, it evolves";
    }
}
