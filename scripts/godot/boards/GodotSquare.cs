using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotSquare : ColorRect
{
    [Export] public GodotPiece GdPiece;
    [Export] public Vector2I Pos;

    private bool lastMoveHighlight, selectedMoveHighlight;
    
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

    public void SetSelectedMoveHighlight(bool state)
    {
        selectedMoveHighlight = state;
        
        Modulate = state ? Colors.LightBlue : Colors.White;
        if (!state && lastMoveHighlight)
            SetLastMoveHighlight(lastMoveHighlight);
    }

    public void SetLastMoveHighlight(bool state)
    {
        lastMoveHighlight = state;

        Modulate = state ? Colors.Yellow : Colors.White;
    }

    public void Clear()
    {
        SetSelectedMoveHighlight(false);
        SetLastMoveHighlight(false);
        
        if (GdPiece is not null)
            GdPiece.QueueFree();
        GdPiece = null;
    }
}
