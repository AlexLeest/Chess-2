using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class MovePieceEvent(byte pieceId, Vector2Int to, bool triggersEvents = true) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        // TODO: Add ON_MOVE item triggers

        Piece piece = board.GetPiece(pieceId);
        // XOR out the hash for this piece at old position
        ZobristCalculator.AdjustZobristHash(piece, board);

        // Change position, adjust board properly
        board.Squares[piece.Position.X, piece.Position.Y] = null;
        board.Squares[to.X, to.Y] = piece;
        piece.Position = to;

        // XOR in hash for piece at new position
        ZobristCalculator.AdjustZobristHash(piece, board);
    }
}
