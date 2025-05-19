using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCastle;

[GlobalClass]
public partial class GDSpawnPawnFence : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCastle.SpawnPawnFence(pieceId);
    }

    public override string GetDescription()
    {
        return "Spawn pawn fence\nOn castling, 3 pawns will spawn in front of the king (where possible)";
    }
}
