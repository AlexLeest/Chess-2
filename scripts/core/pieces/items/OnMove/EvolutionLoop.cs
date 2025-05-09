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
    public override Board Execute(Board board, Move move)
    {
        Piece moved = board.GetPiece(PieceId);
        moved.Upgrade();
        return board;
    }
}
