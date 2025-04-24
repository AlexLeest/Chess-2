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

    public void Clear()
    {
        if (GdPiece is not null)
            GdPiece.QueueFree();
        GdPiece = null;
    }
}
