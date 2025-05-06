using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class UpgradeChoiceButton : Button
{
    [Export] private UpgradeType type;
    [Export] public GodotItem Upgrade;
    [Export] public GodotMovement Movement;
    [Export] public PieceResource Piece;

    public void SetUpgrade(GodotItem upgrade)
    {
        Upgrade = upgrade;
        Text = upgrade.GetDescription();
    }
}

public enum UpgradeType
{
    ITEM,
    MOVEMENT,
    PIECE,
}