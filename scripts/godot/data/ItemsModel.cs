using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class ItemsModel : Resource
{
    [Export] private GodotItem[] items;

    private Dictionary<ItemRarity, List<GodotItem>> itemsByRarity;
    
    public GodotItem GetRandomItemByRarity(ItemRarity itemRarity)
    {
        PopulateDictionary();
        
        List<GodotItem> itemsForRarity = itemsByRarity[itemRarity];
        return itemsForRarity[GD.RandRange(0, itemsForRarity.Count)];
    }

    private void PopulateDictionary()
    {
        if (itemsByRarity is not null)
            return;
        
        itemsByRarity = [];
        
        foreach (GodotItem item in items)
        {
            if (itemsByRarity.TryGetValue(item.Rarity, out List<GodotItem> list))
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
