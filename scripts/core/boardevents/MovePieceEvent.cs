using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class MovePieceEvent(byte pieceId, Vector2Int from, Vector2Int to, bool triggersEvents = true) : IBoardEvent
{
    public readonly byte PieceId = pieceId;
    public readonly Vector2Int From = from;
    public readonly Vector2Int To = to;
    
    public void AdjustBoard(Board board, Move move)
    {
        Piece piece = board.GetPiece(PieceId);
        // XOR out the hash for this piece at old position
        ZobristCalculator.AdjustZobristHash(piece, board);

        // Change position, adjust board properly
        board.Squares.Set(piece.Position, null);//[piece.Position.X, piece.Position.Y] = null;
        board.Squares.Set(To, piece);
        piece.Position = To;

        // XOR in hash for piece at new position
        ZobristCalculator.AdjustZobristHash(piece, board);

        // Handle castling rights
        int colorIndex = board.ColorIndex;
        SpecialPieceTypes pieceType = piece.SpecialPieceType;
        if (board.CastleKingSide[colorIndex] && (pieceType == SpecialPieceTypes.KING_SIDE_CASTLE || pieceType == SpecialPieceTypes.KING))
        {
            board.CastleKingSide[colorIndex] = false;
            board.ZobristHash ^= ZobristCalculator.CastlingHashes[colorIndex];
        }
        if (board.CastleQueenSide[colorIndex] && (pieceType == SpecialPieceTypes.QUEEN_SIDE_CASTLE || pieceType == SpecialPieceTypes.KING))
        {
            board.CastleQueenSide[colorIndex] = false;
            board.ZobristHash ^= ZobristCalculator.CastlingHashes[colorIndex + 2];
        }

        if (triggersEvents)
            board.ActivateItems(PieceId, ItemTriggers.ON_MOVE, board, move, this);
    }
}
