using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class PreparationBoard : GridContainer
{
    // Purpose of this thing:
    // Show ranks 1 and 2 of a board (white starting row) with the player's current setup
    // Allow player to shuffle pieces around at will
    [Export] private PieceTextures pieceTextures;
    [Export] private PlayerSetup boardPlayerSetup;
    
    private GodotSquare[,] squares;

    private PieceResource selectedPiece;
    private GodotSquare highlightedSquare;
    private PieceTooltip tooltip;

    [Signal] public delegate void FinishSetupEventHandler();

    public override void _Ready()
    {
        squares = new GodotSquare[8, 2];
        foreach (Node node in GetChildren())
        {
            if (node is not GodotSquare square)
                continue;

            squares[square.Pos.X, square.Pos.Y] = square;
            square.SquareClicked += SquareClicked;
            square.OnMouseEntered += SquareMouseEnter;
            square.OnMouseExited += SquareMouseExit;
        }
        
        tooltip = GetNode<PieceTooltip>("../Tooltip");
        
        RenderPieces();
    }
    
    public override void _Input(InputEvent input)
    {
        if (input is InputEventKey eventKey && eventKey.Pressed)
        {
            if (eventKey.Keycode == Key.Space)
            {
                FinishSetupAndStartLevel();
            }
        }
    }

    private void SquareMouseEnter(Vector2I coords)
    {
        PieceResource mousedOver = boardPlayerSetup.GetPieceOnPosition(coords);
        if (mousedOver is not null)
        {
            tooltip.ShowTooltip(mousedOver);
        }
    }

    private void SquareMouseExit(Vector2I coords)
    {
        tooltip.HideTooltip();
    }

    public bool SetItem(GodotItem item, Vector2I coords)
    {
        Piece subject = squares[coords.X, coords.Y].GdPiece?.Piece;
        if (subject is not null)
            return boardPlayerSetup.SetItem(subject.Position.ToGodot(), item);
        return false;
    }

    private void SquareClicked(Vector2I position)
    {
        // De-highlight square if one was selected before this
        
        PieceResource clickedPiece = boardPlayerSetup.GetPieceOnPosition(position);
        if (clickedPiece is not null && clickedPiece.PieceType == BasePiece.KING)
        {
            GD.Print("Can't move the king, nope");
            selectedPiece = null;
            return;
        }
        
        if (selectedPiece is not null)
        {
            if (clickedPiece is not null)
            {
                clickedPiece.StartPosition = selectedPiece.StartPosition;
            }
            selectedPiece.StartPosition = position;
            selectedPiece = null;
            RenderPieces();
            return;
        }
        if (clickedPiece is not null)
        {
            selectedPiece = clickedPiece;
            highlightedSquare = squares[position.X, position.Y];
            // Highlight the square

            return;
        }

        selectedPiece = null;
        // No need to select empty squares
    }

    private void RenderPieces()
    {
        foreach (GodotSquare square in squares)
            square.Clear();

        foreach (Piece piece in boardPlayerSetup.GetAsPieces())
        {
            GodotSquare square = squares[piece.Position.X, piece.Position.Y];
            GodotPiece gdPiece = new(piece);
            square.GdPiece = gdPiece;
            square.AddChild(gdPiece);
            gdPiece.Texture = pieceTextures.GetPieceTexture(piece);
        }
    }

    private void FinishSetupAndStartLevel()
    {
        Node canvas = GetTree().CurrentScene;
        // Spawn the "main" scene
        // BoardSetup resource should handle board spawning correctly?

        PackedScene prepBoard = ResourceLoader.Load<PackedScene>("res://prefabs/godot_board.tscn");
        Node board = prepBoard.Instantiate();
        canvas.AddChild(board);
        
        // kys
        QueueFree();
    }
}
