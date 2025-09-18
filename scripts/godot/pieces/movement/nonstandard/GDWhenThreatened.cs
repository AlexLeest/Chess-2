using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GDWhenThreatened : GodotMovement
{
    [Export] private GodotMovement whenThreatened;

    public GDWhenThreatened() { }
    
    public GDWhenThreatened(GodotMovement baseMovement)
    {
        whenThreatened = baseMovement;
    }
    
    public override IMovement GetMovement()
    {
        return new MovementWhenThreatened(whenThreatened.GetMovement());
    }
}
