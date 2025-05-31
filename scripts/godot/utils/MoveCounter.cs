using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class MoveCounter : Node
{
    [Export] private GodotBoard gdBoard;
    [Export] private int depth;
    [Export] private bool debugPrint, countInternalNodes;

    private int internalNodes = 0;
    private int fuckups, good;

    public override void _Input(InputEvent input)
    {
        if (input is InputEventKey eventKey && eventKey.Pressed)
        {
            gdBoard = GetNode<GodotBoard>("../GodotBoard");
            
            internalNodes = countInternalNodes ? 1 : 0;
            if (eventKey.Keycode == Key.Q)
            {
                for (int i = 0; i <= depth; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    int count = CountBoardAmounts(gdBoard.Board, i);
                    GD.Print($"Depth: {i}, Count: {count}, Time: {sw.ElapsedMilliseconds} ms");
                }
            }
            if (eventKey.Keycode == Key.W)
            {
                fuckups = good = 0;
                Stopwatch sw = Stopwatch.StartNew();
                int nodeCount = CountBoardAmounts(gdBoard.Board, depth, debugPrint);
                long time = sw.ElapsedMilliseconds;
                GD.Print($"Depth: {depth}, Count: {nodeCount}, Time: {time} ms, NPS: {nodeCount/(time/1000f)}");
                GD.Print($"Unique zobrist hashes: {zobristCollisionCheck.Count}, good {good}, fuckups {fuckups}");
            }
        }
    }

    private Dictionary<uint, Board> zobristCollisionCheck = [];

    private int CountBoardAmounts(Board currentBoard, int depth, bool print = false)
    {
        uint zobristHash = currentBoard.GetZobristHash();
        if (currentBoard.ZobristHash == zobristHash)
            good++;
        else
            fuckups++;
        if (zobristCollisionCheck.TryGetValue(zobristHash, out Board board))
        {
            // if (!currentBoard.Equals(board))
            //     fuckups++;
            // else
            //     good++;
        }
        else
        {
            zobristCollisionCheck[zobristHash] = currentBoard;
        }
        
        if (depth <= 0)
        {
            return 1;
        }

        // ConcurrentBag<int> counts = new();
        //
        // Parallel.ForEach(currentBoard.GenerateMoves(),
        //     nextBoard =>
        //     {
        //         int count = CountBoardAmounts(nextBoard, depth - 1, false);
        //         if (print)
        //         {
        //             // TODO: Get a last-move field on the board for debug reasons (also visualisation maybe)
        //             GD.Print($"{nextBoard.LastMove}: {count}");
        //         }
        //         counts.Add(count);
        //     }
        // );
        // return counts.Sum() + internalNodes;

        int count = 0;
        foreach (Board nextBoard in currentBoard.GenerateMoves())
        {
            int localCount = CountBoardAmounts(nextBoard, depth - 1, false);
            if (print)
            {
                GD.Print($"{nextBoard.LastMove}: {localCount}");
            }
            count += localCount;
        }
        return count + internalNodes;
    }
}
