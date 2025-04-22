using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public class SelfDestruct(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURED)
{
    public override bool ConditionsMet(Board board, Vector2Int position)
    {
        return true;
    }

    public override Board Execute(Board board, Vector2Int position)
    {
        Piece toBeDestroyed = board.Squares[position.X, position.Y];
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
        board.Squares[position.X, position.Y] = null;
        
        return board;
    }
}
