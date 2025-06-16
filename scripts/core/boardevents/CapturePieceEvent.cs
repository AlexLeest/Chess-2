using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public readonly struct CapturePieceEvent(byte pieceId, Dictionary<byte, IItem[]> itemDict) : IBoardEvent
{
    public void AdjustBoard(Board board)
    {
        Piece piece = board.GetPiece(pieceId);
        
        Piece[] newPieces = new Piece[board.Pieces.Length - 1];
        int index = 0;
        foreach (Piece toCopy in board.Pieces)
        {
            if (toCopy.Id == piece.Id)
                continue;
            newPieces[index] = toCopy;
            index++;
        }
        board.Pieces = newPieces;
        board.Squares[piece.Position.X, piece.Position.Y] = null;
        
        // XOR out piece hash
        board.ZobristHash ^= piece.GetZobristHash();
        
        // XOR out items
        if (itemDict.TryGetValue(piece.Id, out IItem[] items))
            foreach (IItem item in items)
                board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }

    // public uint AdjustZobristHash(uint zobristHash)
    // {
    //     // XOR out piece hash
    //     zobristHash ^= piece.GetZobristHash();
    //     
    //     if (!itemDict.TryGetValue(piece.Id, out IItem[] items))
    //         return zobristHash;
    //
    //     // XOR out items
    //     foreach (IItem item in items)
    //         zobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    //
    //     return zobristHash;
    // }
}
