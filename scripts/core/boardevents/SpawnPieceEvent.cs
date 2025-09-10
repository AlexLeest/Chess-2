using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class SpawnPieceEvent(Piece piece) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        board.AddPiece(piece);
    }
}
