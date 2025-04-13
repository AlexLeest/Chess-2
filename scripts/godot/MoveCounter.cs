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

    private int CountBoardAmounts(Board currentBoard, int depth)
    {
        if (depth == 0)
        {
            return 1;
        }

        int count = 0;
        ConcurrentBag<int> counts = new();

        Parallel.ForEach(currentBoard.GenerateMoves(),
            nextBoard => { counts.Add(CountBoardAmounts(nextBoard, depth - 1)); }
        );
        return counts.Sum();

        // foreach (Board nextBoard in currentBoard.GenerateMoves())
        // {
        //     count += CountBoardAmounts(nextBoard, depth - 1);
        // }

        return count;
    }
}
