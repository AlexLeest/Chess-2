using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;

/// <summary>
/// If the movement was in a straight line or diagonal, pushes pieces away in that direction as well
/// </summary>
/// <param name="pieceId"></param>
public class Pusher(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_MOVE)
{

    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        // Check if the movement was Cardinal or Diagonal
        if (trigger is not MovePieceEvent movePieceEvent)
            return false;
        
        Vector2Int delta = movePieceEvent.To - movePieceEvent.From;
        // Cardinal
        if (delta.X == 0 || delta.Y == 0)
            return true;
        // Diagonal
        if (delta.X == delta.Y || delta.X == -delta.Y)
            return true;

        return false;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        // Get MovePieceEvent out of trigger
        if (trigger is not MovePieceEvent movePieceEvent)
            return board;
        
        int boardWidth = board.Squares.GetLength(0);
        int boardHeight = board.Squares.GetLength(1);

        Vector2Int delta = movePieceEvent.To - movePieceEvent.From;
        Vector2Int pushDirection = new(int.Sign(delta.X), int.Sign(delta.Y));
        Vector2Int pushPos = movePieceEvent.To + pushDirection;
        Vector2Int pushGoal = pushPos + pushDirection;

        if (!pushPos.Inside(boardWidth, boardHeight) || !pushGoal.Inside(boardWidth, boardHeight))
            return board;

        Piece toPush = board.Squares.Get(pushPos);
        if (toPush is null)
            return board;
        if (board.Squares.Get(pushGoal) is not null)
            return board;

        move.ApplyEvent(new MovePieceEvent(toPush.Id, toPush.Position, pushGoal));

        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new Pusher(pieceId);
    }
}
