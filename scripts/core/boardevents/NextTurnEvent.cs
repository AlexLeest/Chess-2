using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public readonly struct NextTurnEvent : IBoardEvent
{
    public void AdjustBoard(Board board)
    {
        board.Turn++;
        // XORs the color hash, since this flips back and forth per turn
        board.ZobristHash ^= ZobristCalculator.GetZobristHash(true);
    }

    // public uint AdjustZobristHash(uint zobristHash)
    // {
    //     // XORs the color hash, since this flips back and forth per turn
    //     return zobristHash ^ ZobristCalculator.GetZobristHash(true);
    // }
}
