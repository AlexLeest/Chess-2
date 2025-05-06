namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

public partial class LeaveBombOnMove : GodotItem
{
    public override ItemRarity Rarity => ItemRarity.COMMON;

    public override string GetDescription()
    {
        return "Leaves bomb behind on the spot the piece moved from.\nBomb sticks around 1 turn and kills any piece that tries to capture it.";
    }
}
