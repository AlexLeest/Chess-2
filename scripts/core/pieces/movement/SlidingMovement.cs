using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class SlidingMovement : IMovement
{
    // In which direction the piece can jump (single step)
    private Vector2Int[] offsets;
    // How many times that step can be repeated in a single move
    private int multiplier;
    
    // For example, king movement would be offsets: [(1,1),(1,0),(1,-1),(0,1),(0,-1),(-1,1),(-1,0),(-1,-1)] and multiplier: 1

    public SlidingMovement(Vector2Int[] offsets, int multiplier)
    {
        this.offsets = offsets;
        this.multiplier = multiplier;
    }

    public List<Vector2Int> GetUnobstructedMovementOptions(Vector2Int from)
    {
        // Hardcoding the board size in for now at 8x8
        foreach (var offset in offsets)
        {
            Vector2Int currentPos = from;
            for (int i = 0; i < multiplier ; i++)
            {
                
            }
        }
    }
}
