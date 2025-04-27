using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class EnemySetup : Resource
{
    [Export] public PieceResource[] EnemyPieces;
}
