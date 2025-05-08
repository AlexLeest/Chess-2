using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnTurn;

[GlobalClass]
public partial class WanderForward : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnTurn.WanderForward(pieceId);
    }

    public override string GetDescription()
    {
        return "Wander Forward:\nPiece walks forward one step (if possible) when not moved.";
    }
}
