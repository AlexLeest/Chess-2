using CHESS2THESEQUELTOCHESS.scripts.godot.AI;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class EnemySetup : Resource
{
    [Export] public PlayerSetup EnemyPieces;
    [Export] public GodotEngine Engine;
    
    public EnemySetup() { }

    public EnemySetup(PlayerSetup pieces, GodotEngine engine)
    {
        EnemyPieces = pieces;
        Engine = engine;
    }

}
