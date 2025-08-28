using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;

public class ColorConverter(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURE) 
{
    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        if (trigger is not CapturePieceEvent captureEvent)
            return false;
        
        Piece capturedPiece = board.GetPiece(captureEvent.CapturedPieceId);
        if (capturedPiece is null)
            return false;

        Piece piece = board.GetPiece(PieceId);
        if (capturedPiece.Color == piece.Color)
            return false;
        return true;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        if (trigger is not CapturePieceEvent captureEvent)
            return board;
        // Instead of capturing, put the piece back with your own color and don't move your own piece
        Piece piece = board.GetPiece(PieceId);
        
        move.ApplyEvent(new ChangeColorEvent(captureEvent.CapturedPieceId, piece.Color));
        move.ApplyEvent(new MovePieceEvent(PieceId, move.From));

        return board;
    }
}
