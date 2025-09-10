using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

public static class Extensions
{
    public static Vector2I ToGodot(this Vector2Int vector)
    {
        return new Vector2I(vector.X, vector.Y);
    }

    public static Vector2Int ToCore(this Vector2I vector)
    {
        return new Vector2Int(vector.X, vector.Y);
    }
    
    public static T Get<T>(this T[,] array, Vector2I position)
    {
        return array[position.X, position.Y];
    }

    public static void Set<T>(this T[,] array, T value, Vector2I position)
    {
        array[position.X, position.Y] = value;
    }
}
