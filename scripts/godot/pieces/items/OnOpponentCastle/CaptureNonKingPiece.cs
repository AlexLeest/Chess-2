using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnOpponentCastle;

[GlobalClass]
public partial class CaptureNonKingPiece : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnOpponentCastle.CaptureNonKingPiece(pieceId);
    }

    public override string GetDescription()
    {
        return "Capture Non-King Piece\nWhen the opponent castles, their non-king piece is destroyed";
    }
}
