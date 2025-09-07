using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class MovePieceEvent(byte pieceId, Vector2Int to, bool triggersEvents = true) : IBoardEvent
{
    public readonly byte PieceId = pieceId;
    public readonly Vector2Int To = to;
    
    public void AdjustBoard(Board board, Move move)
    {
        // TODO: Add ON_MOVE item triggers
        // TODO: Handle castling rights
        // TODO: Add pawn promotion

        Piece piece = board.GetPiece(PieceId);
        // XOR out the hash for this piece at old position
        ZobristCalculator.AdjustZobristHash(piece, board);

        // Change position, adjust board properly
        board.Squares[piece.Position.X, piece.Position.Y] = null;
        board.Squares[To.X, To.Y] = piece;
        piece.Position = To;

        // XOR in hash for piece at new position
        ZobristCalculator.AdjustZobristHash(piece, board);

        board.ActivateItems(PieceId, ItemTriggers.ON_MOVE, board, move, this);
    }
}
