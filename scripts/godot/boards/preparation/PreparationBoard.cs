using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class PreparationBoard : GridContainer
{
    // Purpose of this thing:
    // Show ranks 1 and 2 of a board (white starting row) with the player's current setup
    // Allow player to shuffle pieces around at will
    [Export] private PieceTextures pieceTextures;
    [Export] private PlayerSetup boardPlayerSetup;

    [Export] private UpgradesModel upgradesModel;
    
    private GodotSquare[,] squares;

    private UpgradeChoiceButton[] upgradeButtons;
    private PieceResource selectedPiece;
    private GodotSquare highlightedSquare;

    private Button ContinueButton;
    private PieceTooltip tooltip;
    private bool upgradeMode = true;

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
        UpgradeButtonsSetup();
        ContinueButton = GetNode<Button>("../ContinueButton");
        ContinueButton.Visible = true;
        ContinueButton.Pressed += FinishSetupAndStartLevel;
        
        tooltip = GetNode<PieceTooltip>("../Tooltip");
        
        RenderPieces();
    }

    private void UpgradeButtonsSetup()
    {
        upgradeButtons = GetNode<Node>("../Upgrades").GetChildren().Cast<UpgradeChoiceButton>().ToArray();

        foreach (UpgradeChoiceButton upgradeButton in upgradeButtons)
        {
            ItemRarity rarity = upgradesModel.GetWeightedRandomItemRarity();
            upgradeButton.SetUpgrade(upgradesModel, rarity);
        }
        upgradeButtons[0].GetParent<Container>().Visible = true;
    }
    
    // public override void _Input(InputEvent input)
    // {
    //     if (input is InputEventKey eventKey && eventKey.Pressed)
    //         if (eventKey.Keycode == Key.Space)
    //             FinishSetupAndStartLevel();
    // }

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

    private void SquareClicked(Vector2I coords)
    {
        // De-highlight square if one was selected before this
        
        PieceResource clickedPiece = boardPlayerSetup.GetPieceOnPosition(coords);
        // Check if an upgrade was selected, if so, apply to piece on clicked square
        if (upgradeMode)
        {
            bool upgradeApplied = HandleUpgrade(coords);
            if (upgradeApplied)
            {
                upgradeButtons[0].GetParent<Container>().Visible = false;
                upgradeMode = false;
            }
            return;
        }
        
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
            selectedPiece.StartPosition = coords;
            selectedPiece = null;
            RenderPieces();
            return;
        }
        if (clickedPiece is not null)
        {
            selectedPiece = clickedPiece;
            highlightedSquare = squares[coords.X, coords.Y];
            // Highlight the square

            return;
        }

        selectedPiece = null;
        // No need to select empty squares
    }

    private bool HandleUpgrade(Vector2I coords)
    {
        UpgradeChoiceButton pressed = upgradeButtons.FirstOrDefault(button => button.IsPressed());

        if (pressed is null)
            return false;

        bool upgradeApplied;
        switch (pressed.Type)
        {
            case UpgradeType.ITEM:
                upgradeApplied = boardPlayerSetup.SetItem(coords, pressed.Item);
                break;
            case UpgradeType.MOVEMENT:
                upgradeApplied = boardPlayerSetup.SetMovement(coords, pressed.Movement);
                break;
            case UpgradeType.PIECE:
                upgradeApplied = boardPlayerSetup.AddPiece(coords, pressed.Piece);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        RenderPieces();
        return upgradeApplied;
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
        ContinueButton.Visible = false;
        upgradeButtons[0].GetParent<Container>().Visible = false;
        
        Node canvas = GetTree().CurrentScene;
        // Spawn the "main" scene
        GodotBoard.Level++;

        PackedScene prepBoard = ResourceLoader.Load<PackedScene>("res://prefabs/godot_board.tscn");
        Node board = prepBoard.Instantiate();
        canvas.AddChild(board);
        
        // kys
        QueueFree();
    }
}
