using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

/// <summary>
/// Needs to show the type, movements and items a piece has when it's moused over
/// </summary>
[GlobalClass]
public partial class PieceTooltip : VBoxContainer
{
    [Export] private RichTextLabel pieceNameLabel, movementsLabel, itemsLabel;

    public void ShowTooltip(PieceResource piece)
    {
        Visible = true;
        pieceNameLabel.Text = piece.PieceType.ToString();
        movementsLabel.Text = "";
        itemsLabel.Text = "";
    }

    public void HideTooltip()
    {
        Visible = false;
    }
}
