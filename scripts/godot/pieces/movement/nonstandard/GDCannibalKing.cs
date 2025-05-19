using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GDCannibalKing : GodotMovement
{
    public override IMovement GetMovement()
    {
        return new CannibalKing();
    }
}
