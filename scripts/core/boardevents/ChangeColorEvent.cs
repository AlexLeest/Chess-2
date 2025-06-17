using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class ChangeColorEvent(byte pieceId, bool color) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece piece = board.GetPiece(pieceId);

        if (piece.Color == color)
            return;
        
        // XOR out current piece hash, since the color changing switches everything
        board.ZobristHash ^= piece.GetZobristHash();
        if (board.ItemsPerPiece.TryGetValue(pieceId, out IItem[] items))
            foreach (IItem item in items)
                board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);

        piece.Color = color;
        
    }
}
