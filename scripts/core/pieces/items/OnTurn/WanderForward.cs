using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;

/// <summary>
/// Every turn, this piece attempts to "wander" forward
/// </summary>
/// <param name="pieceId"></param>
public class WanderForward(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_TURN)
{
    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        if (move.Events[0] is MovePieceEvent movePieceEvent && movePieceEvent.PieceId == PieceId)
            return false;
        
        // foreach (IBoardEvent ev in move.Events)
        //     if (ev is MovePieceEvent movePieceEvent && movePieceEvent.PieceId == PieceId)
        //         return false;
        
        Piece piece = board.GetPiece(PieceId);
        if (piece is null)
            return false;
        return true;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        Piece piece = board.GetPiece(PieceId);
        Vector2Int forward = new(piece.Position.X, piece.Position.Y + (piece.Color ? 1 : -1));

        if (!forward.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
            return board;

        Piece inFrontOf = board.Squares.Get(forward);
        if (inFrontOf is not null)
            return board;
        
        move.ApplyEvent(new MovePieceEvent(PieceId, piece.Position, forward));

        return board;
    }
}
