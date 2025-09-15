using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.BeforeCapture;

/// <summary>
/// A copycat ability. When capturing a piece, take over all their items and movements
/// </summary>
/// <param name="pieceId"></param>
public class Changeling(byte pieceId) : AbstractItem(pieceId, ItemTriggers.BEFORE_CAPTURE)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        if (trigger is not CapturePieceEvent captureEvent)
            return board;

        Piece piece = board.GetPiece(PieceId);
        Piece captured = board.GetPiece(captureEvent.CapturedPieceId);

        // Safety measure for nullrefs due to other items removing the piece off the board before this
        if (piece == null || captured == null)
            return board;

        foreach (IMovement movement in captured.Movement)
        {
            move.ApplyEvent(new AddMovementEvent(PieceId, movement));
        }
        if (board.ItemsPerPiece.ContainsKey(captureEvent.CapturedPieceId))
        {
            foreach (IItem item in board.ItemsPerPiece[captureEvent.CapturedPieceId])
            {
                move.ApplyEvent(new AddItemEvent(PieceId, item.GetNewInstance(PieceId)));
            }
        }
        move.ApplyEvent(new RemoveItemEvent(PieceId, this));
        
        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new Changeling(pieceId);
    }
}
