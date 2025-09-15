using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;

/// <summary>
/// When this piece is captured, it also captures the piece that took it.
/// </summary>
/// <param name="pieceId"></param>
public class SelfDestruct(byte pieceId) : AbstractItem(pieceId, ItemTriggers.BEFORE_CAPTURED)
{
    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        return trigger is CapturePieceEvent;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        if (trigger is not CapturePieceEvent captureEvent)
            return board;

        Piece captured = move.Result.GetPiece(PieceId);
        Piece capturer = move.Result.GetPiece(captureEvent.CapturingPieceId);

        // NullRef guard
        if (captured is null || capturer is null)
            return board;
        
        // BUG: Check that this doesn't infinite loop if 2 self-destruct pieces capture eachother?
        //  Keep track of whether an item has already been triggered? Maybe delete the selfdestruct after it gets used to avoid this?
        if (capturer.Position == captured.Position)
            move.ApplyEvent(new CapturePieceEvent(capturer.Id, captured.Id));
        
        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new SelfDestruct(pieceId);
    }
}
