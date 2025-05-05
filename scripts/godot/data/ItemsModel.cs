using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class ItemsModel : Resource
{
    [Export] private GodotItem[] items;
    [Export] private GodotMovement[] movements;

    private Dictionary<ItemRarity, List<IHasRarity>> itemsByRarity;
    
    public IHasRarity GetRandomItemByRarity(ItemRarity itemRarity)
    {
        PopulateDictionary();
        
        List<IHasRarity> itemsForRarity = itemsByRarity[itemRarity];
        return itemsForRarity[GD.RandRange(0, itemsForRarity.Count)];
    }

    private void PopulateDictionary()
    {
        if (itemsByRarity is not null)
            return;
        
        itemsByRarity = [];
        
        foreach (GodotItem item in items)
        {
            if (itemsByRarity.TryGetValue(item.Rarity, out List<IHasRarity> list))
            {
                list.Add(item);
            }
            else
            {
                itemsByRarity[item.Rarity] = [item];
            }
        }

        foreach (GodotMovement item in movements)
        {
            if (itemsByRarity.TryGetValue(item.Rarity, out List<IHasRarity> list))
            {
                list.Add(item);
            }
            else
            {
                itemsByRarity[item.Rarity] = [item];
            }
        }
    }
}
