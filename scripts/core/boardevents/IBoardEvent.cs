namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public interface IBoardEvent
{
    // TODO: Use this as a way to track singular changes to the board state, so movement/captures/items changing can be defined as a sequence of BoardEvents
    //  Zobrist hashing will be able to look at every event and change the hash accordingly

    /// <summary>
    /// Changes the board according to this event's specifications
    /// </summary>
    /// <param name="board">Board to be adjusted</param>
    /// <param name="move">Move this event is being performed on</param>
    public void AdjustBoard(Board board, Move move);
}
