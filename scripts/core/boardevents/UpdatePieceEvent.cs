using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public readonly struct UpdatePieceEvent(Piece before, Piece after) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece[] newPieces = board.DeepcopyPieces(before.Id);
        newPieces[^1] = after;
        board.Pieces = newPieces;

        board.Squares[before.Position.X, before.Position.Y] = null;
        board.Squares[after.Position.X, after.Position.Y] = after;
        
        board.ZobristHash ^= before.GetZobristHash();
        board.ZobristHash ^= after.GetZobristHash();

        if (board.ItemsPerPiece.TryGetValue(before.Id, out IItem[] items))
            foreach (IItem item in items)
            {
                board.ZobristHash ^= item.GetZobristHash(before.Color, before.Position);
                board.ZobristHash ^= item.GetZobristHash(after.Color, after.Position);
            }
    }
}
