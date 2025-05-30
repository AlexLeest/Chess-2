using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

public class FullRandom : IEngine
{
    Random rng = new();
    
    public Board GenerateNextMove(Board board)
    {
        List<Board> choices = board.GenerateMoves();
        if (choices.Count == 0)
            return board;
        return choices[rng.Next(choices.Count)];
    }

    public float DetermineScore(Board board)
    {
        // Does not use score as a useful metric during its move choice
        return 0f;
    }
}
