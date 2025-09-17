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
    
    [Signal] public delegate void SquareClickedEventHandler(Vector2I coords);
    [Signal] public delegate void OnMouseEnteredEventHandler(Vector2I coords);
    [Signal] public delegate void OnMouseExitedEventHandler(Vector2I coords);

    public override void _Ready()
    {
        // Necessary to have the event carry position data to GodotBoard
        MouseEntered += SquareMouseEntered;
        MouseExited += SquareMouseExited;
    }

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

    public void SquareMouseEntered()
    {
        EmitSignalOnMouseEntered(Pos);
    }
    
    public void SquareMouseExited()
    {
        EmitSignalOnMouseExited(Pos);
    }

    public void SetHighlight(bool state)
    {
        // TODO: Highlight this square for possible movement of selected piece
        Modulate = state ? Colors.Gold : Colors.White;
    }

    public void Clear()
    {
        SetHighlight(false);
        if (GdPiece is not null)
            GdPiece.QueueFree();
        GdPiece = null;
    }
}
