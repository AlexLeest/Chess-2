using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;

/// <summary>
/// When this piece is captured, it also captures the piece that took it.
/// </summary>
/// <param name="pieceId"></param>
public class SelfDestruct(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURED)
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
        
        // BUG: Check that this doesn't infinite loop if 2 self-destruct pieces capture eachother?
        //  Keep track of whether an item has already been triggered? Maybe delete the selfdestruct after it gets used to avoid this?
        if (capturer.Position == captured.Position)
            move.ApplyEvent(new CapturePieceEvent(capturer.Id, captured.Id));

        // Piece piece = move.Result.GetPiece(captureEvent.PieceId);
        
        // Vector2Int moveTo = move.To;
        //
        // Piece toBeDestroyed = board.GetPiece(move.Moving);
        // if (toBeDestroyed is null)
        //     return board;
        // Piece[] newPieces = new Piece[board.Pieces.Length - 1];
        // int i = 0;
        // foreach (Piece piece in board.Pieces)
        // {
        //     if (piece == toBeDestroyed)
        //         continue;
        //     newPieces[i] = piece;
        //     i++;
        // }
        // board.Pieces = newPieces;
        // board.Squares[moveTo.X, moveTo.Y] = null;
        // board = board.LastBoard.ActivateItems(toBeDestroyed.Id, ItemTriggers.ON_CAPTURED, board, move, ref events);
        
        return board;
    }
}
