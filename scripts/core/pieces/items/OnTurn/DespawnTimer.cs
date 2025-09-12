using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;

/// <summary>
/// This piece gets deleted after 1 turn of being alive.
/// </summary>
/// <param name="pieceId"></param>
public class DespawnTimer(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_TURN)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        Piece piece = board.GetPiece(PieceId);
        // Check if this piece already existed on the last board. If not, do nothing.
        if (piece is null || board.LastBoard.GetPiece(PieceId) is null)
            return board;
        
        // Piece captures itself? BUG: Will probably bug the fuck out with selfdestruct looping infinitely but let's find out YES IT DOES
        move.ApplyEvent(new CapturePieceEvent(pieceId, pieceId, false));
        
        return board;
    }
}
