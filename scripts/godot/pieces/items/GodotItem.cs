using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

public partial class GodotItem : Resource
{
    // This thing effectively functions as a superclass for core items to become resources

    public virtual IItem GetItem(byte pieceId)
    {
        throw new System.NotImplementedException();
    }
}
