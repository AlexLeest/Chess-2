using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class CaptureItemsEvents(Piece piece, Dictionary<byte, IItem[]> itemDict) : IBoardEvent
{
    public uint AdjustZobristHash(uint zobristHash)
    {
        if (!itemDict.TryGetValue(piece.Id, out IItem[] items))
            return zobristHash;

        foreach (IItem item in items)
        {
            // XOR out items
            zobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
        }

        return zobristHash;
    }
}
