using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public readonly struct UpdatePieceEvent(Piece before, Piece after, Dictionary<byte, IItem[]> itemDict) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece[] newPieces = board.DeepcopyPieces(before.Id);
        newPieces[^1] = after;
        board.Pieces = newPieces;

        board.Squares[before.Position.X, before.Position.Y] = null;
        board.Squares[after.Position.X, after.Position.Y] = after;
    }

    public uint AdjustZobristHash(uint zobristHash)
    {
        zobristHash ^= before.GetZobristHash();
        zobristHash ^= after.GetZobristHash();

        if (!itemDict.TryGetValue(before.Id, out IItem[] items))
            return zobristHash;

        foreach (IItem item in items)
        {
            zobristHash ^= item.GetZobristHash(before.Color, before.Position);
            zobristHash ^= item.GetZobristHash(after.Color, after.Position);
        }

        return zobristHash;
    }
}
