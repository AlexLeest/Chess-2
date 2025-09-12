using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

// TODO: Untested
public class AddItemEvent(byte pieceId, IItem item) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Dictionary<byte, IItem[]> newItemsPerPiece = [];
        IItem[] newItems;
        if (!board.ItemsPerPiece.TryGetValue(pieceId, out IItem[] itemsForPiece))
        {
            newItems = [item];
            newItemsPerPiece[pieceId] = newItems;
        }
        else
        {
            newItems = new IItem[itemsForPiece.Length + 1];
            itemsForPiece.CopyTo(newItems, 0);
            newItems[^1] = item;
        }
        foreach (KeyValuePair<byte, IItem[]> pair in board.ItemsPerPiece)
        {
            if (pair.Key == pieceId)
                newItemsPerPiece[pieceId] = newItems;
            else
                newItemsPerPiece[pair.Key] = pair.Value;
        }
        board.ItemsPerPiece = newItemsPerPiece;

        Piece piece = board.GetPiece(pieceId);
        board.ZobristHash ^= ZobristCalculator.GetZobristHash(piece.Color, piece.Position, item);
    }
}
