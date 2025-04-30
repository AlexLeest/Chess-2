using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items.OnPromotion;

[GlobalClass]
public partial class PromotionExplosion : GodotItem
{
    public override IItem GetItem(byte pieceId)
    {
        return new core.buffs.OnPromotion.PromotionExplosion(pieceId);
    }
}
