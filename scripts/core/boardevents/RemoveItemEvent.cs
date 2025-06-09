using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public readonly struct RemoveItemEvent(Piece piece, IItem item) : IBoardEvent
{
    public uint AdjustZobristHash(uint zobristHash)
    {
        zobristHash ^= item.GetZobristHash(piece.Color, piece.Position);

        return zobristHash;
    }
}
