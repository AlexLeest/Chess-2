using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.BeforeCapture;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.BeforeCapture;

[GlobalClass]
public partial class GDChangeling : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new Changeling(pieceId);
    }

    public override string GetDescription()
    {
        return "Changeling:\nCopies the movement and items from the first piece you capture.";
    }
}
