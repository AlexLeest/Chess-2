using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class LevelModel : Resource
{
    // To hold all the sequential EnemySetups, which will function as levels. Possibly add random generation down the line.
    [Export] public EnemySetup[] EnemySetups;
}
