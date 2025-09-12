using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnTurn;

[GlobalClass]
public partial class GDDespawnTimer : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new DespawnTimer(pieceId);
    }

    public override string GetDescription()
    {
        return "Despawn:\nThis piece destroys itself after 1 turn.";
    }
}
