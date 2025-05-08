using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GodotCheckersMovement : GodotMovement
{

    public override IMovement GetMovement()
    {
        return new CheckersMovement();
    }

    public override string ToString()
    {
        return "Movement: Checkers";
    }
}
