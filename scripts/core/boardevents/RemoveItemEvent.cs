using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class RemoveItemEvent(byte pieceId, IItem item) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece piece = board.GetPiece(pieceId);
        IItem[] itemsForPiece = board.ItemsPerPiece[piece.Id];
        List<IItem> newItems = [];
        foreach (IItem currentItem in itemsForPiece)
        {
            if (currentItem == item)
                continue;
            newItems.Add(currentItem);
        }
        Dictionary<byte, IItem[]> newItemsPerPiece = [];
        foreach (var pair in board.ItemsPerPiece)
        {
            if (pair.Key == piece.Id)
                newItemsPerPiece[pair.Key] = newItems.ToArray();
            else
                newItemsPerPiece[pair.Key] = pair.Value;
        }
        board.ItemsPerPiece = newItemsPerPiece;
        
        board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }
    
    // public void AdjustZobristHash(Board board)
    // {
    //     Piece piece = board.GetPiece(pieceId);
    //     board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    // }
}
