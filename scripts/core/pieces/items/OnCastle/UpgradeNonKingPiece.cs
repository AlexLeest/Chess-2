using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCastle;

public class UpgradeNonKingPiece(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CASTLE) 
{
    private readonly Dictionary<BasePiece, BasePiece> evolutionSteps = new()
    {
        { BasePiece.PAWN, BasePiece.KNIGHT },
        { BasePiece.KNIGHT, BasePiece.BISHOP },
        { BasePiece.BISHOP, BasePiece.ROOK },
        { BasePiece.ROOK, BasePiece.QUEEN },
        // { BasePiece.QUEEN, BasePiece.KING }
    };

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        // Get non-king piece from castling move
        Piece before = move.To.X == 2 ? move.Result.Squares[3, move.To.Y] : move.Result.Squares[5, move.To.Y];
        Piece after = before.DeepCopy(false);
        after.Upgrade();

        move.ApplyEvent(new UpdatePieceEvent(before, after));
        
        return board;
    }
}
