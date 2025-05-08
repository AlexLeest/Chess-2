using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCastle;

[GlobalClass]
public partial class UpgradeNonKingPiece : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCastle.UpgradeNonKingPiece(pieceId);
    }

    public override string GetDescription()
    {
        return "Upgrade Non-King Piece\nOn castling, the non-king piece is upgraded";
    }
}
