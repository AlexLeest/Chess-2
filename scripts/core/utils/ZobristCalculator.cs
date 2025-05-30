using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class ZobristCalculator
{
    private static Random rng = new();

    private static Dictionary<(bool, Vector2Int, BasePiece ), uint> basePieceHashes = [];
    private static Dictionary<(bool, Vector2Int, SpecialPieceTypes), uint> pieceTypeHashes = [];
    private static Dictionary<(bool, Vector2Int, Type), uint> itemHashes = [];
    private static Dictionary<(bool, Vector2Int, Type), uint> movementHashes = [];

    private static uint[] castlingHashes = [RandomUint(), RandomUint(), RandomUint(), RandomUint()];
    private static uint whiteToMoveHash = RandomUint();

    public static uint GetZobristHash(bool color, Vector2Int position, BasePiece piece)
    {
        if (basePieceHashes.TryGetValue((color, position, piece), out uint hash))
            return hash;
        uint result = RandomUint();
        basePieceHashes[(color, position, piece)] = result;
        return result;
    }

    public static uint GetZobristHash(bool color, Vector2Int position, SpecialPieceTypes type)
    {
        if (pieceTypeHashes.TryGetValue((color, position, type), out uint hash))
            return hash;
        uint result = RandomUint();
        pieceTypeHashes[(color, position, type)] = result;
        return result;
    }

    public static uint GetZobristHash(bool color, Vector2Int position, IItem item)
    {
        if (itemHashes.TryGetValue((color, position, item.GetType()), out uint hash))
            return hash;
        uint result = RandomUint();
        itemHashes[(color, position, item.GetType())] = result;
        return result;
    }

    public static uint GetZobristHash(bool color, Vector2Int position, IMovement movement)
    {
        if (movementHashes.TryGetValue((color, position, movement.GetType()), out uint hash))
            return hash;
        uint result = RandomUint();
        movementHashes[(color, position, movement.GetType())] = result;
        return result;
    }

    public static uint GetZobristHash(bool color)
    {
        return color ? whiteToMoveHash : 0;
    }

    public static uint GetZobristHash(bool[] kingSide, bool[] queenSide)
    {
        uint result = 0;

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

    public static uint RandomUint()
    {
        byte[] buffer = new byte[4];
        rng.NextBytes(buffer);
        return BitConverter.ToUInt32(buffer, 0);
    }
}
