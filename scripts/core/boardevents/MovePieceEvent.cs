using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class MovePieceEvent(byte pieceId, Vector2Int to, Dictionary<byte, IItem[]> itemDict) : IBoardEvent
{
    public void AdjustBoard(Board board)
    {
        Piece piece = board.GetPiece(pieceId);
        // XOR out the hash for this piece at old position
        XorPieceHash(board, piece);

        // Change position, adjust board properly
        board.Squares[piece.Position.X, piece.Position.Y] = null;
        board.Squares[to.X, to.Y] = piece;
        piece.Position = to;

        // XOR in hash for piece at new position
        XorPieceHash(board, piece);
    }

    private void XorPieceHash(Board board, Piece piece)
    {
        board.ZobristHash ^= piece.GetZobristHash();
        if (itemDict.TryGetValue(pieceId, out IItem[] items))
            foreach (IItem item in items)
                board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }
}
