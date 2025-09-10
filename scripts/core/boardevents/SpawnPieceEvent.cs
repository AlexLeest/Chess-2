using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class SpawnPieceEvent(Piece piece) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece[] newPieces = board.DeepcopyPieces();
        newPieces[^1] = piece;
        board.Pieces = newPieces;
        board.Squares.Set(piece.Position, piece);//[piece.Position.X, piece.Position.Y] = piece;

        ZobristCalculator.AdjustZobristHash(piece, board);
        // board.ZobristHash ^= piece.GetZobristHash();
        // if (board.ItemsPerPiece.TryGetValue(piece.Id, out IItem[] items))
        //     foreach (IItem item in items)
        //         board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }
}
