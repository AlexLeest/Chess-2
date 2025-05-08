using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotPiece : TextureRect
{
    [Export] public byte Id;
    public Piece Piece;
    
    public GodotPiece(Piece piece)
    {
        Piece = piece;
        Id = piece.Id;
    }
}