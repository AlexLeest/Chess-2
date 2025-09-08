using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class NextTurnEvent : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        // TODO: Handle pawn promotion
        board.ActivateItems(board.ColorToMove, ItemTriggers.ON_TURN, board, move, this);
        
        board.Turn++;
        // XORs the color hash, since this flips back and forth per turn
        board.ZobristHash ^= ZobristCalculator.GetZobristHash(true);
    }
}
