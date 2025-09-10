using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class NextTurnEvent : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        // TODO: Handle pawn promotion
        board.ActivateItems(board.ColorToMove, ItemTriggers.ON_TURN, board, move, this);

        foreach (Piece piece in board.Pieces)
        {
            if (piece.BasePiece == BasePiece.PAWN && piece.Position.Y == (piece.Color ? 7 : 0))
            {
                Piece promoted = piece.DeepCopy(false).ChangeTo(BasePiece.QUEEN);
                move.ApplyEvent(new UpdatePieceEvent(piece, promoted));
            }
        }
        
        board.Turn++;
        // XORs the color hash, since this flips back and forth per turn
        board.ZobristHash ^= ZobristCalculator.GetZobristHash(true);
    }
}
