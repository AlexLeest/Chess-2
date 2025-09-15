using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;

/// <summary>
/// This piece "evolves" into the next type of piece every time it moves
/// PAWN -> KNIGHT -> BISHOP -> ROOK -> QUEEN -> PAWN
/// </summary>
/// <param name="pieceId"></param>
public class EvolutionLoop(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_MOVE)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        Piece before = move.Result.GetPiece(PieceId);
        Piece after = before.DeepCopy(false);
        if (before.BasePiece == BasePiece.QUEEN)
            after.ChangeTo(BasePiece.PAWN);
        else
            after.Upgrade();
        
        move.ApplyEvent(new UpdatePieceEvent(before, after));
        
        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new EvolutionLoop(pieceId);
    }
}
