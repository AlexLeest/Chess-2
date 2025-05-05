using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class UpgradeChoiceButton : Button
{
    [Export] public GodotItem Upgrade;

    public void SetUpgrade(GodotItem upgrade)
    {
        Upgrade = upgrade;
        Text = upgrade.GetDescription();
    }
}
