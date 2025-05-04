namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;

/// <summary>
/// When piece captures a piece of it's own 
/// </summary>
/// <param name="pieceId"></param>
public class CannibalTeeth(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURE)
{
    public override bool ConditionsMet(Board board, Move move)
    {
        Piece piece = board.GetPiece(PieceId);
        Piece captured = move.Captured;

        return piece.Color == captured.Color;
    }

    public override Board Execute(Board board, Move move)
    {
        return board;
    }
}
