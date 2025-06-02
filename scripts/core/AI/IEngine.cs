namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// Interface for any kind of bot
/// </summary>
public interface IEngine
{
    public Move GenerateNextMove(Board board);
    public float DetermineScore(Board board);
}
