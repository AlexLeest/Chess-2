using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs.OnTurn;

/// <summary>
/// Every turn, this piece attempts to "wander" forward
/// </summary>
/// <param name="pieceId"></param>
public class WanderForward(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_TURN)
{
    public override bool ConditionsMet(Board board, Move move)
    {
        if (move.Moving == PieceId)
            return false;
        Piece piece = board.GetPiece(pieceId);
        if (piece is null)
            return false;
        return true;
    }

    public override Board Execute(Board board, Move move)
    {
        // TODO: All of this I guess
        Piece piece = board.GetPiece(pieceId);
        Vector2Int forward = new Vector2Int(piece.Position.X, piece.Position.Y + (piece.Color ? 1 : -1));

        if (!forward.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
            return board;
        
        Piece inFrontOf = board.Squares[forward.X, forward.Y];
        if (inFrontOf is not null)
            return board;

        board.Squares[piece.Position.X, piece.Position.Y] = null;
        piece.Position = forward;
        board.Squares[forward.X, forward.Y] = piece;

        return board;
    }
}
