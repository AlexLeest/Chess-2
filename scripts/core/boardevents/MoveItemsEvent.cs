using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class MoveItemsEvent(Piece piece, Dictionary<byte, IItem[]> itemDict, Vector2Int from, Vector2Int to) : IBoardEvent
{
    public uint AdjustZobristHash(uint zobristHash)
    {
        if (!itemDict.TryGetValue(piece.Id, out IItem[] items))
            return zobristHash;

        foreach (IItem item in items)
        {
            // XOR out the from pos
            zobristHash ^= item.GetZobristHash(piece.Color, from);
            // XOR in the to pos
            zobristHash ^= item.GetZobristHash(piece.Color, to);
        }

        return zobristHash;
    }
}
