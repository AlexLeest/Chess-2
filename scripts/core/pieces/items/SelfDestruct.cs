using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

/// <summary>
/// When this piece is captured, it also captures the piece that took it.
/// </summary>
/// <param name="pieceId"></param>
public class SelfDestruct(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURED)
{
    public override bool ConditionsMet(Board board, Move move)
    {
        return true;
    }

    public override Board Execute(Board board, Move move)
    {
        Vector2Int moveTo = move.To;
        
        Piece toBeDestroyed = board.Squares[moveTo.X, moveTo.Y];
        Piece[] newPieces = new Piece[board.Pieces.Length - 1];
        int i = 0;
        foreach (Piece piece in board.Pieces)
        {
            if (piece == toBeDestroyed)
                continue;
            newPieces[i] = piece;
            i++;
        }
        board.Pieces = newPieces;
        board.Squares[moveTo.X, moveTo.Y] = null;
        board = board.LastBoard.ActivateItems(toBeDestroyed.Id, ItemTriggers.ON_CAPTURED, board, move);
        
        return board;
    }
}
