using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.BeforeCapture;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnOpponentCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnPromotion;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;
using CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCapture;
using CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCaptured;
using CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnCastle;
using CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnMove;
using CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnOpponentCastle;
using CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnPromotion;
using CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.OnTurn;
using Godot;
using System;
using GDCannibalTeeth = CHESS2THESEQUELTOCHESS.scripts.godot.pieces.items.BeforeCapture.GDCannibalTeeth;
using SelfDestruct = CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured.SelfDestruct;

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
            CannibalTeeth => new GDCannibalTeeth(),
            Gun => new GDGun(),
            Respawn => new GDRespawn(),
            SelfDestruct => new GDSelfDestruct(),
            OpponentRoyalSwap => new GDOpponentRoyalSwap(),
            EvolutionLoop => new GDEvolutionLoop(),
            LeaveBombOnMove => new GDLeaveBombOnMove(),
            PromotionExplosion => new GDPromotionExplosion(),
            KingOfTheHill => new GDKingOfTheHill(),
            WanderForward => new GDWanderForward(),
            CaptureNonKingPiece => new GDCaptureNonKingPiece(),
            UpgradeNonKingPiece => new GDUpgradeNonKingPiece(),
            SpawnPawnFence => new GDSpawnPawnFence(),
            HandHolder => new GDHandHolder(),
            ColorConverter => new GDColorConverter(),
            _ => throw new NotImplementedException(),
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
