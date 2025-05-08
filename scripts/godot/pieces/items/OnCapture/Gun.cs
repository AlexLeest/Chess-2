using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCapture;

[GlobalClass]
public partial class Gun : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCapture.Gun(pieceId);
    }

    public override string GetDescription()
    {
        return "Le gun:\nIt shoot. It hurt.";
    }
}
