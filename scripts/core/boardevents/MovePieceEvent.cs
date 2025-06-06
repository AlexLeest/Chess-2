using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class MovePieceEvent(Piece piece, Vector2Int from, Vector2Int to) : IBoardEvent
{
    public uint AdjustZobristHash(uint zobristHash)
    {
        // XOR out the from pos
        zobristHash ^= piece.GetZobristHash(from);
        // XOR in the to pos
        zobristHash ^= piece.GetZobristHash(to);

        return zobristHash;
    }
}
