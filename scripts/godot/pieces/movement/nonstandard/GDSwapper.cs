using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement.nonstandard;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GDSwapper : GodotMovement
{

    public override IMovement GetMovement()
    {
        return new Swapper();
    }
    
    
}
