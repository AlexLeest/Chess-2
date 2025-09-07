using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;

/// <summary>
/// "Shoots" the enemy piece, not actually moving this piece over there
/// </summary>
/// <param name="pieceId"></param>
public class Gun(byte pieceId) : AbstractItem(pieceId, ItemTriggers.BEFORE_CAPTURE)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        move.ApplyEvent(new MovePieceEvent(PieceId, move.From));

        return board;
    }
}
