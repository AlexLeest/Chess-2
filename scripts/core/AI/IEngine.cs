namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

/// <summary>
/// Interface for any kind of bot
/// </summary>
public interface IEngine
{
    public float DetermineScore(Board board);
    public Board GenerateNextMove(Board board);
}
