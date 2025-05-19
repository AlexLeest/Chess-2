using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;

public class HandHolder(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_MOVE)
{
    private int boardWidth, boardHeight;
    private Piece piece;
    
    public override Board Execute(Board board, Move move)
    {
        // Find adjecent pieces with the same base piece type and color, try to move them with you
        piece = board.GetPiece(PieceId);
        Vector2Int moveDelta = move.To - move.From;

        boardWidth = board.Squares.GetLength(0);
        boardHeight = board.Squares.GetLength(1);

        Vector2Int left = new(move.From.X - 1, move.From.Y);
        board = AttemptPieceMove(board, left, moveDelta);
        Vector2Int right = new(move.From.X + 1, move.From.Y);
        board = AttemptPieceMove(board, right, moveDelta);
        
        return board;
    }

    private Board AttemptPieceMove(Board board, Vector2Int position, Vector2Int delta)
    {
        if (!position.Inside(boardWidth, boardHeight))
            return board;
        Piece toBeMoved = board.Squares[position.X, position.Y];
        if (toBeMoved is null || toBeMoved.Id == PieceId || toBeMoved.BasePiece != piece.BasePiece || toBeMoved.Color != piece.Color)
            return board;
        Vector2Int goalPos = position + delta;
        if (!goalPos.Inside(boardWidth, boardHeight))
            return board;
        if (board.Squares[goalPos.X, goalPos.Y] is not null)
            return board;

        toBeMoved.Position = goalPos;
        board.Squares[position.X, position.Y] = null;
        board.Squares[goalPos.X, goalPos.Y] = toBeMoved;
        board = board.ActivateItems(toBeMoved.Id, ItemTriggers.ON_MOVE, board, new Move(toBeMoved.Id, position, goalPos));

        return board;
    }
}
