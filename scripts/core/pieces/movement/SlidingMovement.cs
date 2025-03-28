using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

// Ayyyy I'm slidin ovah here
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

    public List<Vector2Int> GetMovementOptions(Vector2Int from, Piece[,] pieces, bool color)
    {
        // Hardcoding the board size in for now at 8x8
        List<Vector2Int> result = [];
        
        foreach (var offset in offsets)
        {
            Vector2Int currentPos = from;
            for (int i = 0; i < multiplier ; i++)
            {
                currentPos += offset;
                Piece onSquare = pieces[currentPos.X, currentPos.Y];
                
                if (onSquare != null)
                {
                    if (onSquare.Color != color)
                        result.Add(currentPos);
                    break;
                }
                
                result.Add(currentPos);
            }
        }

        return result;
    }

    public static SlidingMovement Knight => new(Vector2Int.KnightHops, 1);
    public static SlidingMovement Bishop => new(Vector2Int.Diagonals, 8);
    public static SlidingMovement Rook => new(Vector2Int.Cardinals, 8);
    public static SlidingMovement Queen => new(Vector2Int.AllDirections, 8);
    public static SlidingMovement King => new(Vector2Int.AllDirections, 1);

}
