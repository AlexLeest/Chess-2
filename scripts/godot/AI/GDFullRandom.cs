using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.AI;

[GlobalClass]
public partial class GDFullRandom : GodotEngine
{
    public override IEngine GetEngine()
    {
        return new FullRandom();
    }
}
