using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

public abstract partial class GodotMovement : Resource
{
    [Export] public ItemRarity Rarity;

    public abstract IMovement GetMovement();

    public abstract override string ToString();

    public static GodotMovement CreateFromIMovement(IMovement movement)
    {
        switch (movement)
        {
            case PawnMovement:
                return new GodotPawnMovement();
            case CheckersMovement:
                return new GodotCheckersMovement();
            case SlidingMovement slidingMovement:
                List<Vector2> offsets = [];
                foreach (Vector2Int offset in slidingMovement.GetOffsets())
                {
                    offsets.Add(new Vector2(offset.X, offset.Y));
                }
                return new GodotSlidingMovement(offsets.ToArray(), slidingMovement.GetMultiplier());
        }
        throw new System.NotImplementedException();
    }
}
