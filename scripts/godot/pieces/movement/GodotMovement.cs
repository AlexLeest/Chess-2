using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement.nonstandard;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

public abstract partial class GodotMovement : Resource
{
    [Export] public ItemRarity Rarity;

    public abstract IMovement GetMovement();
    
    public static GodotMovement CreateFromIMovement(IMovement movement)
    {
        switch (movement)
        {
            case PawnMovement:
                return new GodotPawnMovement();
            case SlidingMovement slidingMovement:
                List<Vector2> offsets = [];
                foreach (Vector2Int offset in slidingMovement.GetOffsets())
                {
                    offsets.Add(new Vector2(offset.X, offset.Y));
                }
                return new GodotSlidingMovement(offsets.ToArray(), slidingMovement.GetMultiplier());
            case CheckersMovement:
                return new GodotCheckersMovement();
            case CannibalKing:
                return new GDCannibalKing();
            case TriplePawnPush:
                return new GDTriplePawnPush();
            case CastlingMovement:
                return new GodotCastlingMovement();
            case Swapper:
                return new GDSwapper();
        }
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return GetMovement().ToString() ?? string.Empty;
    }
}
