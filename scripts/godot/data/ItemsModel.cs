using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class ItemsModel : Resource
{
    [Export] private GodotItem[] items;
    [Export] private GodotMovement[] movements;

    private Dictionary<ItemRarity, List<GodotItem>> itemsByRarity;
    private Dictionary<ItemRarity, List<GodotMovement>> movementsByRarity;
    private Dictionary<ItemRarity, List<GodotPiece>> piecesByRarity;
    
    public GodotItem GetRandomItemByRarity(ItemRarity itemRarity)
    {
        PopulateItemDictionary();
        
        List<GodotItem> itemsForRarity = itemsByRarity[itemRarity];
        return itemsForRarity[GD.RandRange(0, itemsForRarity.Count - 1)];
    }

    public GodotMovement GetRandomMovementByRarity(ItemRarity itemRarity)
    {
        PopulateMovementDictionary();
        
        List<GodotMovement> movementsForRarity = movementsByRarity[itemRarity];
        return movementsForRarity[GD.RandRange(0, movementsForRarity.Count)];
    }

    private void PopulateItemDictionary()
    {
        if (itemsByRarity is not null)
            return;
        
        itemsByRarity = [];
        
        foreach (GodotItem item in items)
        {
            if (itemsByRarity.TryGetValue(item.Rarity, out List<GodotItem> list))
                list.Add(item);
            else
                itemsByRarity[item.Rarity] = [item];
        }
    }

    private void PopulateMovementDictionary()
    {
        if (movementsByRarity is not null)
            return;
        
        movementsByRarity = [];
        
        foreach (GodotMovement item in movements)
        {
            if (movementsByRarity.TryGetValue(item.Rarity, out List<GodotMovement> list))
                list.Add(item);
            else
                movementsByRarity[item.Rarity] = [item];
        }
    }
}
