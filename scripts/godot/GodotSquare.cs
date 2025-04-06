using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotSquare : ColorRect
{
    [Export] public GodotPiece GdPiece;
    [Export] public Vector2I Pos;
    
    [Export] private int squareNumber;
    private Color color;
    
    [Signal] public delegate void SquareClickedEventHandler(Vector2I position);

    public override void _Ready()
    {
        // Set square with color and whatnot
    }

    // public override void _Draw()
    // {
    //     var pos = new Vector2(position.X * 64, 64 * 8 - ((position.Y+1) * 64));
    //     Rect2 rect = new(pos, 64, 64);
    //     float greyscale = (position.X + Position.Y * 8) / 64;
    //     DrawRect(rect, new Color(greyscale, greyscale, greyscale, 1f));
    // }

    public override void _GuiInput(InputEvent input)
    {    
        if (input is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == MouseButton.Left && mb.Pressed)
            {
                // GD.Print($"Click pos {Pos}");
                EmitSignalSquareClicked(Pos);
            }
        }
    }

    public void SetPiece(GodotPiece piece)
    {
        this.GdPiece = piece;
    }
}
