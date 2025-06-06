namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class CapturePieceEvent(Piece piece) : IBoardEvent
{

    public uint AdjustZobristHash(uint zobristHash)
    {
        // XOR out piece hash
        zobristHash ^= piece.GetZobristHash();

        return zobristHash;
    }
}
