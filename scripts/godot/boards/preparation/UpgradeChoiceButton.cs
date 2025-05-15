using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class UpgradeChoiceButton : Button
{
    [Export] public UpgradeType Type;
    [Export] public GodotItem Item;
    [Export] public GodotMovement Movement;
    [Export] public BasePiece Piece;

    public void SetUpgrade(UpgradesModel model, ItemRarity rarity)
    {
        switch (Type)
        {
            case UpgradeType.ITEM:
                Item = model.GetRandomItemByRarity(rarity);
                Text = Item.GetDescription();
                break;
            case UpgradeType.MOVEMENT:
                Movement = model.GetRandomMovementByRarity(rarity);
                Text = $"Movement:\n{Movement}";
                break;
            case UpgradeType.PIECE:
                Piece = model.GetRandomPieceByRarity(rarity);
                Text = Piece.ToString();
                break;
        }
    }
}

public enum UpgradeType
{
    ITEM,
    MOVEMENT,
    PIECE,
}