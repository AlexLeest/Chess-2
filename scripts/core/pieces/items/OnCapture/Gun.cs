using CHESS2THESEQUELTOCHESS.scripts.core.buffs;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

/// <summary>
/// "Shoots" the enemy piece, not actually moving this piece over there
/// </summary>
/// <param name="pieceId"></param>
public class Gun(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURE) {
    public override bool ConditionsMet(Board board, Move move)
    {
        return true;
    }

    public override Board Execute(Board board, Move move)
    {
        Piece piece = board.GetPiece(PieceId);
        
        // Set the position back to FROM
        piece.Position = move.From;
        // Reset the squares as well
        board.Squares[move.To.X, move.To.Y] = null;
        board.Squares[move.From.X, move.From.Y] = piece;

        return board;
    }
}
