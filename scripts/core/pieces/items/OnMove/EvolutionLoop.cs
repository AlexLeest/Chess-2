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
    public override Board Execute(Board board, Move move, ref List<IBoardEvent> events)
    {
        Piece moved = board.GetPiece(PieceId);
        if (moved.BasePiece == BasePiece.QUEEN)
            moved.ChangeTo(BasePiece.PAWN);
        else
            moved.Upgrade();
        return board;
    }
}
