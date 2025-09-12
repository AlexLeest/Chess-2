using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class CapturePieceEvent(byte capturedPieceId, byte capturingPieceId, bool triggersEvents = true) : IBoardEvent
{
    public readonly byte CapturedPieceId = capturedPieceId;
    public readonly byte CapturingPieceId = capturingPieceId;
    
    public void AdjustBoard(Board board, Move move)
    {
        if (board.GetPiece(CapturedPieceId) is null)
            return;
        
        if (triggersEvents)
        {
            board.ActivateItems(CapturingPieceId, ItemTriggers.BEFORE_CAPTURE, board, move, this);
            board.ActivateItems(CapturedPieceId, ItemTriggers.BEFORE_CAPTURED, board, move, this);
        }
        
        // Removal of captured piece
        board.RemovePiece(CapturedPieceId);
        
        // Activating items
        if (triggersEvents)
        {
            board.ActivateItems(CapturingPieceId, ItemTriggers.AFTER_CAPTURE, board, move, this);
            board.ActivateItems(CapturedPieceId, ItemTriggers.AFTER_CAPTURED, board, move, this);
        }
    }
}
