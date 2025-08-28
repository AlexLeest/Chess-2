using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;

/// <summary>
/// If piece is in centre of the board, "evolve" it to a stronger piece
/// PAWN -> KNIGHT -> BISHOP -> ROOK -> QUEEN -> KING??
/// </summary>
/// <param name="pieceId"></param>
public class KingOfTheHill(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_TURN)
{
    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        if (move.Moving == PieceId)
            return false;
        
        Piece piece = board.GetPiece(PieceId);
        if (piece?.Position.X is >= 3 and <= 4 && piece.Position.Y is >= 3 and <= 4)
            return true;
        
        return false;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        Piece before = board.GetPiece(PieceId);
        Piece after = before.DeepCopy(false);
        after.Upgrade();
        move.ApplyEvent(new UpdatePieceEvent(before, after));
        return board;
    }
}
