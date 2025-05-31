using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

public class TranspositionTable
{
    // private Dictionary<uint, Entry> table = [];
    private Dictionary<uint, Entry> entries = [];

    private int count;

    // public TranspositionTable(int sizeMB = 312)
    // {
    //     int entrySizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Entry));
    //     int desiredTableSize = sizeMB * 1024 * 1024;
    //
    //     count = desiredTableSize / entrySizeInBytes;
    //     
    //     entries = new Entry[count];
    // }

    public bool TryGetEntry(uint zobristHash, out Entry entry)
    {
        return entries.TryGetValue(zobristHash, out entry);
    }

    public void AddEntry(uint zobristHash, int depth, float score, Board[] bestMoves)
    {
        Entry entry = new(zobristHash, depth, score, bestMoves);
        entries[zobristHash] = entry;
    }

    private int GetIndex(uint zobristHash)
    {
        return (int)(zobristHash % count);
    }
}

public struct Entry(uint zobristHash, int depth, float score, Board[] bestMoves)
{
    public uint ZobristHash = zobristHash;
    public int Depth = depth;
    public float Score = score;
    public Board[] BestMoves = bestMoves;
}