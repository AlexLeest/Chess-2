using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class ZobristCalculator
{
    private static Random rng = new();

    private static Dictionary<(bool, Vector2Int, BasePiece ), int> basePieceHashes = [];
    private static Dictionary<(bool, Vector2Int, SpecialPieceTypes), int> pieceTypeHashes = [];
    private static Dictionary<(bool, Vector2Int, Type), int> itemHashes = [];

    private static int[] castlingHashes = [];

    public static int GetZobristHash(bool color, Vector2Int position, BasePiece piece)
    {
        if (basePieceHashes.TryGetValue((color, position, piece), out int hash))
            return hash;
        int result = rng.Next(int.MinValue, int.MaxValue);
        basePieceHashes[(color, position, piece)] = result;
        return result;
    }

    public static int GetZobristHash(bool color, Vector2Int position, SpecialPieceTypes type)
    {
        if (pieceTypeHashes.TryGetValue((color, position, type), out int hash))
            return hash;
        int result = rng.Next(int.MinValue, int.MaxValue);
        pieceTypeHashes[(color, position, type)] = result;
        return result;
    }

    public static int GetZobristHash(bool color, Vector2Int position, IItem item)
    {
        if (itemHashes.TryGetValue((color, position, item.GetType()), out int hash))
            return hash;
        int result = rng.Next(int.MinValue, int.MaxValue);
        itemHashes[(color, position, item.GetType())] = result;
        return result;
    }

    public static int GetZobristHash(bool[] kingSide, bool[] queenSide)
    {
        if (castlingHashes.Length == 0)
        {
            castlingHashes = new int[4];
            for (int i = 0; i < 4; i++)
                castlingHashes[i] = rng.Next(int.MinValue, int.MaxValue);
        }
        int result = 0;

        if (kingSide[0])
            result ^= castlingHashes[0];
        if (kingSide[1])
            result ^= castlingHashes[1];
        if (queenSide[0])
            result ^= castlingHashes[2];
        if (queenSide[1])
            result ^= castlingHashes[3];

        return result;
    }
}
