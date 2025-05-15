using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnOpponentCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnPromotion;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.items;

public abstract partial class GodotItem : Resource
{
    // This thing effectively functions as a superclass for core items to become resources
    [Export] public ItemRarity Rarity;

    public abstract IItem GetItem(byte pieceId);

    public abstract string GetDescription();

    public static GodotItem CreateFromIItem(IItem item)
    {
        return item switch
        {
            CannibalTeeth => new pieces.items.OnCapture.CannibalTeeth(),
            Gun => new pieces.items.OnCapture.Gun(),
            Respawn => new pieces.items.OnCaptured.Respawn(),
            SelfDestruct => new pieces.items.OnCaptured.SelfDestruct(),
            OpponentRoyalSwap => new pieces.items.OnCastle.OpponentRoyalSwap(),
            EvolutionLoop => new pieces.items.OnMove.EvolutionLoop(),
            LeaveBombOnMove => new pieces.items.OnMove.LeaveBombOnMove(),
            PromotionExplosion => new pieces.items.OnPromotion.PromotionExplosion(),
            KingOfTheHill => new pieces.items.OnTurn.KingOfTheHill(),
            WanderForward => new pieces.items.OnTurn.WanderForward(),
            CaptureNonKingPiece => new pieces.items.OnOpponentCastle.CaptureNonKingPiece(),
            UpgradeNonKingPiece => new pieces.items.OnCastle.UpgradeNonKingPiece(),
            SpawnPawnFence => new pieces.items.OnCastle.SpawnPawnFence(),
            _ => throw new System.NotImplementedException(),
        };

    }
}

public enum ItemRarity
{
    // 90% chance for common
    COMMON,
    // 9% chance for rare
    RARE,
    // 1% chance for legendary
    LEGENDARY,
}
