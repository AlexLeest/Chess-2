using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class SpawnPieceEvent(Piece piece) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece[] newPieces = board.DeepcopyPieces();
        newPieces[^1] = piece;
        board.Squares[piece.Position.X, piece.Position.Y] = piece;
        
        board.ZobristHash ^= piece.GetZobristHash();
        if (board.ItemsPerPiece.TryGetValue(piece.Id, out IItem[] items))
            foreach (IItem item in items)
                board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }

    // public uint AdjustZobristHash(uint zobristHash)
    // {
    //     zobristHash ^= piece.GetZobristHash();
    //     if (!itemDict.TryGetValue(piece.Id, out IItem[] items))
    //         return zobristHash;
    //
    //     foreach (IItem item in items)
    //         zobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    //
    //     return zobristHash;
    // }
}
