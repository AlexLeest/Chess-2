using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using Godot;
using System;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class LevelModel : Resource
{
    // To hold all the sequential EnemySetups, which will function as levels. Possibly add random generation down the line.
    [Export] public EnemySetup[] EnemySetups;

    public PieceResource[] GetPieces(int level)
    {
        level = Math.Clamp(level, 0, EnemySetups.Length - 1);
        return EnemySetups[level].EnemyPieces;
    }

    public IEngine GetEngine(int level)
    {
        level = Math.Clamp(level, 0, EnemySetups.Length - 1);
        return EnemySetups[level].Engine.GetEngine();
    }
}
