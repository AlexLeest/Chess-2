using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using System;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.AI;

public class FullRandom : IEngine
{
    Random rng = new();
    
    public Move GenerateNextMove(Board board)
    {
        List<Move> choices = board.GetMoves();
        if (choices.Count == 0)
            return new Move();
        Move move = choices[rng.Next(choices.Count)];
        
        List<IBoardEvent> events = [];
        while (board.ApplyMove(move, out _) is null)
            move = choices[rng.Next(choices.Count)];
        return move;
    }

    public float DetermineScore(Board board)
    {
        // Does not use score as a useful metric during its move choice
        return 0f;
    }
}
