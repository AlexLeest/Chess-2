using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class CastleEvent(bool color) : IBoardEvent
{
    public readonly bool Color = color;

    public void AdjustBoard(Board board, Move move)
    {
        int colorIndex = Color ? 0 : 1;
        bool[] kingSide = move.Result.CastleKingSide;
        if (kingSide[colorIndex])
        {
            kingSide[colorIndex] = false;
            board.ZobristHash ^= ZobristCalculator.CastlingHashes[colorIndex];
        }
        bool[] queenSide = move.Result.CastleQueenSide;
        if (queenSide[colorIndex])
        {
            queenSide[colorIndex] = false;
            board.ZobristHash ^= ZobristCalculator.CastlingHashes[2 + colorIndex];
        }

        board.CastleKingSide = kingSide;
        board.CastleQueenSide = queenSide;
        
        board.ActivateItems(board.ColorToMove, ItemTriggers.ON_CASTLE, board, move, this);
        board.ActivateItems(!board.ColorToMove, ItemTriggers.ON_OPPONENT_CASTLE, board, move, this);
    }
}
