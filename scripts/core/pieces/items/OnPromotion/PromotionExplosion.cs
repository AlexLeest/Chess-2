using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnPromotion;

/// <summary>
/// Causes an explosion around the point of promotion, capturing any pieces surrounding this spot.
/// </summary>
/// <param name="pieceId"></param>
public class PromotionExplosion(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_PROMOTION)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        Vector2Int target = move.To;
        int boardWidth = board.Squares.GetLength(0);
        int boardHeight = board.Squares.GetLength(1);

        for (int x = target.X - 1; x <= target.X + 1; x++)
            for (int y = target.Y - 1; y <= target.Y + 1; y++)
            {
                if (x == target.X && y == target.Y)
                    continue;
                Vector2Int toKillPos = new(x, y);
                if (!toKillPos.Inside(boardWidth, boardHeight))
                    continue;
                
                Piece toKill = board.Squares.Get(toKillPos);
                if (toKill is null || toKill.SpecialPieceType == SpecialPieceTypes.KING)
                    continue;

                move.ApplyEvent(new CapturePieceEvent(toKill.Id, PieceId));
            }

        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new PromotionExplosion(pieceId);
    }
}
