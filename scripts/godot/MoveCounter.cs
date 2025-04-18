using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class MoveCounter : Node
{
    [Export] private GodotBoard gdBoard;
    [Export] private int depth;
    [Export] private bool debugPrint;

    public override void _Input(InputEvent input)
    {
        if (input is InputEventKey eventKey && eventKey.Pressed)
        {
            if (eventKey.Keycode == Key.Q)
            {
                for (int i = 0; i <= depth; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    GD.Print($"Depth: {i}, Count: {CountBoardAmounts(gdBoard.Board, i)}, Time: {sw.ElapsedMilliseconds} ms");
                }
            }
            if (eventKey.Keycode == Key.W)
            {
                Stopwatch sw = Stopwatch.StartNew();
                GD.Print($"Depth: {depth}, Count: {CountBoardAmounts(gdBoard.Board, depth, debugPrint)}, Time: {sw.ElapsedMilliseconds} ms");
            }
        }
    }

    private int CountBoardAmounts(Board currentBoard, int depth, bool print = false)
    {
        if (depth <= 0)
        {
            return 1;
        }

        ConcurrentBag<int> counts = new();
        
        Parallel.ForEach(currentBoard.GenerateMoves(),
            nextBoard =>
            {
                int count = CountBoardAmounts(nextBoard, depth - 1, false);
                if (print)
                {
                    // TODO: Get a last-move field on the board for debug reasons (also visualisation maybe)
                    GD.Print($"{nextBoard.LastMove}: {count}");
                }
                counts.Add(count);
            }
        );
        return counts.Sum();

        // int count = 0;
        // foreach (Board nextBoard in currentBoard.GenerateMoves())
        // {
        //     int localCount = CountBoardAmounts(nextBoard, depth - 1, false);
        //     if (print)
        //     {
        //         // TODO: Get a last-move field on the board for debug reasons (also visualisation maybe)
        //         GD.Print($"{nextBoard.LastMove}: {localCount}");
        //     }
        //     count += localCount;
        // }
        // return count;
    }
}
