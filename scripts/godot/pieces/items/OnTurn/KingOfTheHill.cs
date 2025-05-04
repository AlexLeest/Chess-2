using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnTurn;

[GlobalClass]
public partial class KingOfTheHill : GodotItem
{
    public override Rarity Rarity => Rarity.COMMON;

    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnTurn.KingOfTheHill(pieceId);
    }

    public override string GetDescription()
    {
        return "King of the Hill:\nPiece evolves to the next rank up while standing still in the center of the board.";
    }
}
