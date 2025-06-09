using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public readonly struct CapturePieceEvent(Piece piece, Dictionary<byte, IItem[]> itemDict) : IBoardEvent
{
    public uint AdjustZobristHash(uint zobristHash)
    {
        // XOR out piece hash
        zobristHash ^= piece.GetZobristHash();
        
        if (!itemDict.TryGetValue(piece.Id, out IItem[] items))
            return zobristHash;

        // XOR out items
        foreach (IItem item in items)
            zobristHash ^= item.GetZobristHash(piece.Color, piece.Position);

        return zobristHash;
    }
}
