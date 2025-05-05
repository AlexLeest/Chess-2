using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnPromotion;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

public abstract partial class GodotItem : Resource, IUnlockableItem
{
    // This thing effectively functions as a superclass for core items to become resources
    public abstract ItemRarity Rarity { get; }

    public virtual IItem GetItem(byte pieceId)
    {
        throw new System.NotImplementedException();
    }

    public virtual string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public static GodotItem CreateFromIItem(IItem item)
    {
        switch (item)
        {
            case core.pieces.items.OnCapture.CannibalTeeth:
                return new pieces.items.OnCapture.CannibalTeeth();
            case core.pieces.items.OnCapture.Gun:
                return new Gun();
            case core.pieces.items.OnCaptured.Respawn:
                return new Respawn();
            case core.pieces.items.OnCaptured.SelfDestruct:
                return new SelfDestruct();
            case core.pieces.items.OnCastle.OpponentRoyalSwap:
                return new OpponentRoyalSwap();
            case core.pieces.items.OnMove.EvolutionLoop:
                return new EvolutionLoop();
            case core.pieces.items.OnMove.LeaveBombOnMove:
                return new LeaveBombOnMove();
            case PromotionExplosion:
                return new pieces.items.OnPromotion.PromotionExplosion();
            case KingOfTheHill:
                return new pieces.items.OnTurn.KingOfTheHill();
            case WanderForward:
                return new OnTurn.WanderForward();
        }
        
        throw new System.NotImplementedException();
    }
}

public enum ItemRarity
{
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY,
}
