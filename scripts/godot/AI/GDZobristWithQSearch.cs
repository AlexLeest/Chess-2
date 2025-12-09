using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.AI;

[GlobalClass]
public partial class GDZobristWithQSearch : GodotEngine
{
    [Export] private int depth;

    public override IEngine GetEngine()
    {
        return new ZobristWithQSearch(depth);
    }
}
