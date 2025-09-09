using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
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
        ZobristCalculator.AdjustZobristHash(piece, board);

        // Change color, XOR in new hash
        piece.Color = color;
        ZobristCalculator.AdjustZobristHash(piece, board);
    }
}
