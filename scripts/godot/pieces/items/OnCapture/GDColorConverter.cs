using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCapture;

[GlobalClass]
public partial class GDColorConverter : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnCapture.ColorConverter(pieceId);
    }

    public override string GetDescription()
    {
        return "Color Converter:\nInstead of capturing a piece, it'll be converted to your color";
    }
}
