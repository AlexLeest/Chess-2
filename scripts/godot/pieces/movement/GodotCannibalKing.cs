using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GodotCannibalKing : GodotMovement
{
    public override IMovement GetMovement()
    {
        return new CannibalKing();
    }

    public override string ToString()
    {
        return "Cannibal:\nCan capture friendly pieces right next to it without moving";
    }
}
