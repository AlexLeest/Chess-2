using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs.OnPromotion;

public class PromotionExplosion(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_PROMOTION) {
    public override bool ConditionsMet(Board board, Move move)
    {
        return true;
    }

    public override Board Execute(Board board, Move move)
    {
        List<Piece> piecesToKill = new List<Piece>();
        Vector2Int target = move.To;

        for (int x = target.X - 1; x <= target.X + 1; x++)
        {
            for (int y = target.Y - 1; y <= target.Y + 1; y++)
            {
                if (x == target.X && y == target.Y)
                    continue;
                Vector2Int toKillPos = new Vector2Int(x, y);
                if (!toKillPos.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
                    continue;
                
                Piece toKill = board.Squares[toKillPos.X, toKillPos.Y];
                if (toKill is null)
                    continue;

                board = board.ActivateItems(toKill.Id, ItemTriggers.ON_CAPTURED, board, move);
                // TODO: The rest of this owl
            }
        }

        return board;
    }
}
