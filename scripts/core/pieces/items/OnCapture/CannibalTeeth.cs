using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;

/// <summary>
/// When piece captures a piece of its own color, evolve the piece
/// </summary>
/// <param name="pieceId"></param>
public class CannibalTeeth(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURE)
{
    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        Piece piece = move.Result.GetPiece(PieceId);
        if (trigger is not CapturePieceEvent captureEvent)
            return false;

        Piece captured = move.Result.GetPiece(captureEvent.CapturedPieceId);
        return piece.Color == captured.Color;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        Piece before = board.GetPiece(PieceId);
        
        // Don't want to actually change the piece itself, copy and work on those
        Piece after = before.DeepCopy(false);
        after.Upgrade();
        
        move.ApplyEvent(new UpdatePieceEvent(before, after));
        
        return board;
    }
}
