namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class Extensions
{
    public static T Get<T>(this T[,] array, Vector2Int position)
    {
        return array[position.X, position.Y];
    }

    public static void Set<T>(this T[,] array, Vector2Int position, T value)
    {
        array[position.X, position.Y] = value;
    }
}
