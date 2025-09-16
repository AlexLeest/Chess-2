using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnMove;

[GlobalClass]
public partial class GDPusher: GodotItem
{

    public override IItem GetItem(byte pieceId)
    {
        return new Pusher(pieceId);
    }

    public override string GetDescription()
    {
        return "Pusher:\nWhen moving in a straight line or diagonally, pushes the piece in front of where it lands away.";
    }
}
