using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class GodotMovement : Resource
{
    [Export] private MovementType type;
    // Array of Vector2I not allowed so this is a quick workaround
    [Export] private Vector2[] offsets;
    [Export] private int multiplier;

    public IMovement GetMovement()
    {
        if (type == MovementType.PAWN)
        {
            return new PawnMovement();
        }
        if (type == MovementType.CHECKERS)
        {
            return new CheckersJump();
        }
        
        List<Vector2Int> intOffsets = [];
        foreach (Vector2 offset in offsets)
        {
            intOffsets.Add(new Vector2Int((int)offset.X, (int)offset.Y));
        }
        return new SlidingMovement(intOffsets.ToArray(), multiplier);
    }
}

public enum MovementType
{
    PAWN,
    SLIDING,
    CHECKERS,
}
