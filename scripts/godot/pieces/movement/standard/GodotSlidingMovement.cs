using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GodotSlidingMovement : GodotMovement
{
    [Export] private Vector2[] offsets;
    [Export] private int multiplier;

    public GodotSlidingMovement() { }

    public GodotSlidingMovement(Vector2[] offsets, int multiplier)
    {
        this.offsets = offsets;
        this.multiplier = multiplier;
    }

    public override IMovement GetMovement()
    {
        List<Vector2Int> intOffsets = [];
        foreach (Vector2 offset in offsets)
        {
            intOffsets.Add(new Vector2Int((int)offset.X, (int)offset.Y));
        }
        return new SlidingMovement(intOffsets.ToArray(), multiplier);
    }
}
