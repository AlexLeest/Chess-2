using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnPromotion;

[GlobalClass]
public partial class PromotionExplosion : GodotItem
{
    public override Rarity Rarity => Rarity.COMMON;
    
    public override IItem GetItem(byte pieceId)
    {
        return new core.pieces.items.OnPromotion.PromotionExplosion(pieceId);
    }

    public override string GetDescription()
    {
        return "Promotion Explosion:\nThis piece captures the surrounding spaces when it promotes.";
    }
}
