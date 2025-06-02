using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GodotCastlingMovement : GodotMovement
{
    public override IMovement GetMovement()
    {
        return new CastlingMovement();
    }
}
