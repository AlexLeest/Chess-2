namespace CHESS2THESEQUELTOCHESS.scripts.core;

public interface IBoardEvent
{
    // TODO: Use this as a way to track singular changes to the board state, so movement/captures/items changing can be defined as a sequence of BoardEvents
    //  Zobrist hashing will be able to look at every event and change the hash accordingly

    public uint AdjustZobristHash(uint zobristHash);
}
