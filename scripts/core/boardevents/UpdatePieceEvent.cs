namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class UpdatePieceEvent(Piece before, Piece after) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        board.RemovePiece(before.Id);
        board.AddPiece(after);
    }
}
