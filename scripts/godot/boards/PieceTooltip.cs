using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;

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
        List<string> moveTexts = [];
        foreach (GodotMovement movement in piece.Movement)
        {
            // Or do this as separate new rich text entries
            moveTexts.Add(movement.ToString());
        }
        movementsLabel.Text = string.Join("\n", moveTexts);
        List<string> itemsTexts = [];
        foreach (GodotItem item in piece.Items)
        {
            // Or do this as separate new rich text entries
            itemsTexts.Add(item.GetDescription());
        }
        itemsLabel.Text = string.Join("\n", itemsTexts);
    }

    public void HideTooltip()
    {
        Visible = false;
    }
}
