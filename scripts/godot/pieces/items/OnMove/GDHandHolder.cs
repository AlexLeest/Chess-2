using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnMove;

[GlobalClass]
public partial class GDHandHolder : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new HandHolder(pieceId);
    }

    public override string GetDescription()
    {
        return "Hand Holder:\nWhen moving, this piece will try and move pieces of the same type next to it along";
    }
}
