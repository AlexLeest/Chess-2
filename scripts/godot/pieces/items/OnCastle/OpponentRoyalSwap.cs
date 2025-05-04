using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

[GlobalClass]
public partial class OpponentRoyalSwap : GodotItem
{
    public override ItemRarity Rarity => ItemRarity.COMMON;

    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCastle.OpponentRoyalSwap(pieceId);
    }

    public override string GetDescription()
    {
        return "Royal Swap:\nWhen you castle, the opponent's king and queen swap their places";
    }
}
