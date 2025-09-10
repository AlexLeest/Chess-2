using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class UpdatePieceEvent(Piece before, Piece after) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        ZobristCalculator.AdjustZobristHash(before, board);
        
        Piece[] newPieces = board.DeepcopyPieces(before.Id);
        newPieces[^1] = after;
        board.Pieces = newPieces;

        board.Squares.Set(before.Position, null);//before.Position.X, before.Position.Y] = null;
        board.Squares.Set(after.Position, after);//[after.Position.X, after.Position.Y] = after;

        ZobristCalculator.AdjustZobristHash(after, board);

        // board.ZobristHash ^= before.GetZobristHash();
        // board.ZobristHash ^= after.GetZobristHash();
        //
        // if (board.ItemsPerPiece.TryGetValue(before.Id, out IItem[] items))
        //     foreach (IItem item in items)
        //     {
        //         board.ZobristHash ^= item.GetZobristHash(before.Color, before.Position);
        //         board.ZobristHash ^= item.GetZobristHash(after.Color, after.Position);
        //     }
    }
}
