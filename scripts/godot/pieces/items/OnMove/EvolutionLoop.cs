using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

[GlobalClass]
public partial class EvolutionLoop : GodotItem
{
    public override ItemRarity Rarity => ItemRarity.COMMON;

    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnMove.EvolutionLoop(pieceId);
    }

    public override string GetDescription()
    {
        return "Evolution Loop:\nThis piece evolves when moved, looping back from queen to pawn.";
    }
}
