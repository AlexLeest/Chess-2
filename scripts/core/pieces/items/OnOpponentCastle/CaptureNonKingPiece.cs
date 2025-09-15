using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnOpponentCastle;

public class CaptureNonKingPiece(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_OPPONENT_CASTLE)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        // Get non-king piece in castling, KILL
        int xCoord = move.To.X;
        Piece nonKingPiece;
        if (xCoord == 2)
            nonKingPiece = board.Squares[3, move.To.Y];
        else
            nonKingPiece = board.Squares[5, move.To.Y];
        
        if (nonKingPiece is null)
            return board;

        move.ApplyEvent(new CapturePieceEvent(nonKingPiece.Id, PieceId));
        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new CaptureNonKingPiece(pieceId);
    }
}
