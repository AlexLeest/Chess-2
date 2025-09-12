using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.BeforeCapture;

/// <summary>
/// "Shoots" the enemy piece, not actually moving this piece over there
/// </summary>
/// <param name="pieceId"></param>
public class Gun(byte pieceId) : AbstractItem(pieceId, ItemTriggers.BEFORE_CAPTURE)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        Piece piece = board.GetPiece(PieceId);
        move.ApplyEvent(new MovePieceEvent(PieceId, piece.Position, move.From, false));

        return board;
    }
}
