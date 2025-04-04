using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotPiece : Sprite2D
{
    [Export] public byte Id;
    // [Export] public Sprite2D Sprite;
    private Piece piece;
    
    public GodotPiece(Piece piece)
    {
        Update(piece);
    }

    public void Update(Piece piece)
    {
        // GD.Print($"Moving piece {piece.Id} to pos {piece.Position.X}, {piece.Position.Y}");
        Id = piece.Id;
        this.piece = piece;
        // Has to flip the board upside down to have the white pieces at the bottom: 64*8 - position
        Position = new Vector2(32 + piece.Position.X * 64, 512 - (32 + piece.Position.Y * 64));
    }
}