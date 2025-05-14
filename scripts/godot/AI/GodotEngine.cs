using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.AI;

public abstract partial class GodotEngine : Resource
{
    public abstract IEngine GetEngine();
}
