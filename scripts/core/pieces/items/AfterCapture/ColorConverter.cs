using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.AfterCapture;

public class ColorConverter(byte pieceId) : AbstractItem(pieceId, ItemTriggers.AFTER_CAPTURE) 
{
    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        if (trigger is not CapturePieceEvent captureEvent)
            return false;
        
        Piece capturedPiece = board.LastBoard.GetPiece(captureEvent.CapturedPieceId);
        if (capturedPiece is null)
            return false;

        Piece piece = board.GetPiece(PieceId);
        if (piece is null)
            return false;
        if (capturedPiece.Color == piece.Color)
            return false;
        return true;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        if (trigger is not CapturePieceEvent captureEvent)
            return board;
        // Instead of capturing, put the piece back with your own color and don't move your own piece
        // Problem with that is that it's getting removed right after this in the CapturePieceEvent anyway, so uhhhh
        Piece piece = board.GetPiece(PieceId);
        Piece capturedPiece = board.LastBoard.GetPiece(captureEvent.CapturedPieceId).DeepCopy(false);
        capturedPiece.Color = piece.Color;
        
        move.ApplyEvent(new MovePieceEvent(PieceId, piece.Position, move.From, false));
        move.ApplyEvent(new SpawnPieceEvent(capturedPiece));

        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new ColorConverter(pieceId);
    }
}
