using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;

/// <summary>
/// When piece captures a piece of its own color, evolve the piece
/// </summary>
/// <param name="pieceId"></param>
public class CannibalTeeth(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURE)
{
    public override bool ConditionsMet(Board board, Move move)
    {
        Piece piece = board.GetPiece(PieceId);
        Piece captured = move.Captured;

        return piece.Color == captured.Color;
    }

    public override Board Execute(Board board, Move move, ref List<IBoardEvent> events)
    {
        Piece piece = board.GetPiece(PieceId);
        Piece before = piece.DeepCopy(false);
        
        piece.Upgrade();
        events.Add(new UpdatePieceEvent(before, piece, board.ItemsPerPiece));
        
        return board;
    }
}
