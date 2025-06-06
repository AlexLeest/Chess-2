using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class SwitchColorEvent(Piece before, Piece after, Dictionary<byte, IItem[]> itemDict) : IBoardEvent
{
    public uint AdjustZobristHash(uint zobristHash)
    {
        // XOR out the piece hash
        zobristHash ^= before.GetZobristHash();
        // XOR in the new piece
        zobristHash ^= after.GetZobristHash();

        if (itemDict.TryGetValue(before.Id, out IItem[] items))
        {
            foreach (IItem item in items)
            {
                // XOR out item
                zobristHash ^= item.GetZobristHash(before.Color, before.Position);
                // XOR in item
                zobristHash ^= item.GetZobristHash(after.Color, after.Position);
            }
        }
        
        return zobristHash;
    }
}
