using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnPromotion;

/// <summary>
/// Causes an explosion around the point of promotion, capturing any pieces surrounding this spot.
/// </summary>
/// <param name="pieceId"></param>
public class PromotionExplosion(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_PROMOTION)
{
    public override Board Execute(Board board, Move move)
    {
        Vector2Int target = move.To;

        for (int x = target.X - 1; x <= target.X + 1; x++)
            for (int y = target.Y - 1; y <= target.Y + 1; y++)
            {
                if (x == target.X && y == target.Y)
                    continue;
                Vector2Int toKillPos = new(x, y);
                if (!toKillPos.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
                    continue;
                
                Piece toKill = board.Squares[toKillPos.X, toKillPos.Y];
                if (toKill is null || toKill.SpecialPieceType == SpecialPieceTypes.KING)
                    continue;

                // Remove piece from pieces list and squares repr
                List<Piece> newPieces = [];
                foreach (Piece piece in board.Pieces)
                {
                    if (piece.Id == toKill.Id)
                        continue;
                    newPieces.Add(piece);
                }
                board.Pieces = newPieces.ToArray();
                board.Squares[toKillPos.X, toKillPos.Y] = null;
                
                // Activate ON_CAPTURED items for that piece
                board = board.ActivateItems(toKill.Id, ItemTriggers.ON_CAPTURED, board, move);
            }

        return board;
    }
}
