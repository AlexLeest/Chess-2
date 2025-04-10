using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

// TODO: Struct, maybe
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

    public List<Vector2Int> GetMovementOptions(Vector2Int from, Piece[,] squares, bool color)
    {
        int boardWidth = squares.GetLength(0);
        int boardHeight = squares.GetLength(1);
        
        // Hardcoding the board size in for now at 8x8
        List<Vector2Int> options = [];
        
        foreach (var offset in offsets)
        {
            Vector2Int currentPos = from;
            for (int i = 0; i < multiplier ; i++)
            {
                currentPos += offset;
                if (!currentPos.Inside(boardWidth, boardHeight))
                    break;

                Piece onSquare = squares[currentPos.X, currentPos.Y];
                if (onSquare != null)
                {
                    if (onSquare.Color != color)
                        options.Add(currentPos);
                    break;
                }
                
                options.Add(currentPos);
            }
        }

        return options;
    }

    public static SlidingMovement Knight => new(Vector2Int.KnightHops, 1);
    public static SlidingMovement Bishop => new(Vector2Int.Diagonals, 7);
    public static SlidingMovement Rook => new(Vector2Int.Cardinals, 7);
    public static SlidingMovement Queen => new(Vector2Int.AllDirections, 7);
    public static SlidingMovement King => new(Vector2Int.AllDirections, 1);

}
