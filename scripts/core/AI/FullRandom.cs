using System;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

public class FullRandom : IEngine
{
    public Board GenerateNextMove(Board board)
    {
        Random rnd = new();
        var choices = board.GenerateMoves();
        return choices[rnd.Next(choices.Count)];
    }

    public float DetermineScore(Board board)
    {
        // Does not use score as a useful metric during its move choice
        return 0f;
    }
}
