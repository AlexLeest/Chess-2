using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
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

    [Export] public ItemRarity Rarity;

    public IMovement GetMovement()
    {
        if (type == MovementType.PAWN)
        {
            return new PawnMovement();
        }
        if (type == MovementType.CHECKERS)
        {
            return new CheckersMovement();
        }
        
        List<Vector2Int> intOffsets = [];
        foreach (Vector2 offset in offsets)
        {
            intOffsets.Add(new Vector2Int((int)offset.X, (int)offset.Y));
        }
        return new SlidingMovement(intOffsets.ToArray(), multiplier);
    }

    public override string ToString()
    {
        if (type is MovementType.SLIDING)
        {
            return $"Movement: {type},\noffsets: {string.Join(", ", offsets)},\nmultiplier: {multiplier}";
        }

        return $"Movement: {type}";
    }

    public static GodotMovement CreateFromIMovement(IMovement movement)
    {
        GodotMovement result = new();
        switch (movement)
        {
            case PawnMovement:
                result.type = MovementType.PAWN;
                break;
            case CheckersMovement:
                result.type = MovementType.CHECKERS;
                break;
            case SlidingMovement slidingMovement:
                result.type = MovementType.SLIDING;
                List<Vector2> offsets = [];
                foreach (Vector2Int offset in slidingMovement.GetOffsets())
                {
                    offsets.Add(new Vector2(offset.X, offset.Y));
                }
                result.offsets = offsets.ToArray();
                result.multiplier = slidingMovement.GetMultiplier();
                break;
        }

        return result;
    }
}

public enum MovementType
{
    PAWN,
    SLIDING,
    CHECKERS,
}
