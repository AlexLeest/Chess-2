using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.AfterCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.BeforeCapture;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnOpponentCastle;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnPromotion;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

public abstract class AbstractItem(byte pieceId, ItemTriggers trigger) : IItem
{
    protected Dictionary<(bool, Vector2Int), int> hashValues = [];
    private Random rng = new();
    
    public byte PieceId { get; } = pieceId;
    public ItemTriggers Trigger { get; } = trigger;

    public virtual bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        return true;
    }

    public abstract Board Execute(Board board, Move move, IBoardEvent trigger);

    public abstract IItem GetNewInstance(byte pieceId);

    public virtual uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    // public static IItem GetNewInstance(IItem instance, byte pieceId)
    // {
    //     return instance switch
    //     {
    //         AfterCapture.ColorConverter => new AfterCapture.ColorConverter(pieceId),
    //         Respawn => new Respawn(pieceId),
    //         CannibalTeeth => new CannibalTeeth(pieceId),
    //         Changeling => new Changeling(pieceId),
    //         Gun => new Gun(pieceId),
    //         SelfDestruct => new SelfDestruct(pieceId),
    //         OpponentRoyalSwap => new OpponentRoyalSwap(pieceId),
    //         SpawnPawnFence => new SpawnPawnFence(pieceId),
    //         UpgradeNonKingPiece => new UpgradeNonKingPiece(pieceId),
    //         EvolutionLoop => new EvolutionLoop(pieceId),
    //         HandHolder => new HandHolder(pieceId),
    //         LeaveBombOnMove => new LeaveBombOnMove(pieceId),
    //         CaptureNonKingPiece => new CaptureNonKingPiece(pieceId),
    //         PromotionExplosion => new PromotionExplosion(pieceId),
    //         DespawnTimer => new DespawnTimer(pieceId),
    //         KingOfTheHill => new KingOfTheHill(pieceId),
    //         WanderForward => new WanderForward(pieceId),
    //         _ => throw new NotImplementedException(),
    //     };
    // }
}
