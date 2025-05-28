using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

public class TranspositionTable
{
    // TODO: Find a way to track collisions (even if just for debug purposes)
    private Dictionary<int, Entry> table = [];

    public bool TryGetEntry(int zobristHash, out Entry entry)
    {
        return table.TryGetValue(zobristHash, out entry);
    }

    public void AddEntry(int zobristHash, int depth, float score, Board[] bestMoves)
    {
        Entry entry = new(zobristHash, depth, score, bestMoves);
        table[zobristHash] = entry;
    }
}

public struct Entry(int zobristHash, int depth, float score, Board[] bestMoves)
{
    public int ZobristHash = zobristHash;
    public int Depth = depth;
    public float Score = score;
    public Board[] BestMoves = bestMoves;
}