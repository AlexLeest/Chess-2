using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class ZobristCalculator
{
    private static Random rng = new();
    
    private static Dictionary<(bool, BasePiece, Vector2Int), int> basePieceHashes = [];
    private static Dictionary<(bool, SpecialPieceTypes, Vector2Int), int> pieceTypeHashes = [];

    public static int GetZobristHash(bool color, BasePiece piece, Vector2Int position)
    {
        if (basePieceHashes.TryGetValue((color, piece, position), out int hash))
            return hash;
        int result = rng.Next(int.MinValue, int.MaxValue);
        basePieceHashes[(color, piece, position)] = result;
        return result;
    }

    public static int GetZobristHash(bool color, SpecialPieceTypes type, Vector2Int position)
    {
        if (pieceTypeHashes.TryGetValue((color, type, position), out int hash))
            return hash;
        int result = rng.Next(int.MinValue, int.MaxValue);
        pieceTypeHashes[(color, type, position)] = result;
        return result;
    }
}
