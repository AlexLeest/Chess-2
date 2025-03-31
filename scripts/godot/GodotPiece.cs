using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotPiece : Sprite2D
{
    private Piece piece;
    [Export] private Sprite2D sprite;
}
