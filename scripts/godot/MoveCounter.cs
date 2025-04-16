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

    public override void _Input(InputEvent input)
    {
        if (input is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.Q)
        {
            for (int i = 0; i <= depth; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                GD.Print($"Depth: {i}, Count: {CountBoardAmounts(gdBoard.Board, i)}, Time: {sw.ElapsedMilliseconds} ms");
            }
        }
    }

    private int CountBoardAmounts(Board currentBoard, int depth, bool print = true)
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
                    GD.Print($"{count} moves");
                }
                counts.Add(count);
            }
        );
        return counts.Sum();

        // foreach (Board nextBoard in currentBoard.GenerateMoves())
        // {
        //     count += CountBoardAmounts(nextBoard, depth - 1);
        // }
    }
}
