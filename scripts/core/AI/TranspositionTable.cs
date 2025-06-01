using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

public class TranspositionTable
{
    private Dictionary<uint, Entry> entries = [];

    public bool TryGetEntry(uint zobristHash, out Entry entry)
    {
        return entries.TryGetValue(zobristHash, out entry);
    }

    public void AddEntry(uint zobristHash, int depth, float score, Board[] bestMoves)
    {
        Entry entry = new(zobristHash, depth, score, bestMoves);
        entries[zobristHash] = entry;
    }

    public void Clear()
    {
        entries.Clear();
    }
}

public struct Entry(uint zobristHash, int depth, float score, Board[] bestMoves)
{
    public uint ZobristHash = zobristHash;
    public int Depth = depth;
    public float Score = score;
    public Board[] BestMoves = bestMoves;
}