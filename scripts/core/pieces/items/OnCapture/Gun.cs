using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;

/// <summary>
/// "Shoots" the enemy piece, not actually moving this piece over there
/// </summary>
/// <param name="pieceId"></param>
public class Gun(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURE)
{
    public override Board Execute(Board board, Move move, ref List<IBoardEvent> events)
    {
        Piece piece = board.GetPiece(PieceId);
        MovePieceEvent ev = new MovePieceEvent(piece.Id, move.From);
        move.ApplyEvent(ev);
        
        // Piece before = board.GetPiece(PieceId);
        // Piece after = before.DeepCopy(false);
        // after.Position = move.From;
        //
        // // Set the position back to FROM
        // before.Position = move.From;
        // // Reset the squares as well
        // board.Squares[move.To.X, move.To.Y] = null;
        // board.Squares[move.From.X, move.From.Y] = before;

        return board;
    }
}
