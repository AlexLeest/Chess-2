using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GodotPawnMovement : GodotMovement
{
    public override IMovement GetMovement()
    {
        return new PawnMovement();
    }

    public override string ToString()
    {
        return "Movement: Pawn";
    }
}
