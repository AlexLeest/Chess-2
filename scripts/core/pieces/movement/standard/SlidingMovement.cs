using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

/// <summary>
/// Generic movement. The offsets are the "directions" the piece can move in, a maximum of "multiplier" amount of times
/// Supports every base piece movement except pawns.
/// </summary>
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

    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        Piece[,] squares = board.Squares;
        
        int boardWidth = squares.GetLength(0);
        int boardHeight = squares.GetLength(1);
        
        // Hardcoding the board size in for now at 8x8
        List<Move> options = [];
        
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
                        options.Add(new Move(id, from, currentPos, onSquare));
                    break;
                }

                options.Add(new Move(id, from, currentPos));
            }
        }

        return options;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        Piece[,] squares = board.Squares;
        
        int boardWidth = squares.GetLength(0);
        int boardHeight = squares.GetLength(1);
        
        // Maybe break up SlidingMovement to only have 1 offset instead of a list. idk.
        Vector2Int delta = target - from;
        foreach (Vector2Int offset in offsets)
        {
            Vector2Int currentPos = from;
            // Check if signs line up, if not, skip
            if (Math.Sign(offset.X) != Math.Sign(delta.X) || Math.Sign(offset.Y) != Math.Sign(delta.Y))
                continue;

            for (int i = 0; i < multiplier; i++)
            {
                currentPos += offset;
                if (currentPos == target)
                {
                    // Hit target square (regardless of piece existing there or not)
                    return true;
                }
                if (!currentPos.Inside(boardWidth, boardHeight))
                    break;
                
                Piece onSquare = squares[currentPos.X, currentPos.Y];
                if (onSquare != null)
                {
                    break;
                }
            }
        }

        return false;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        Piece[,] squares = board.Squares;
        
        int boardWidth = squares.GetLength(0);
        int boardHeight = squares.GetLength(1);
        
        // Maybe break up SlidingMovement to only have 1 offset instead of a list. idk.
        Vector2Int[] deltas = new Vector2Int[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            Vector2Int target = targets[i];
            deltas[i] = target - from;
        }
        foreach (Vector2Int offset in offsets)
        {
            // Check if signs line up, if not, skip
            for (int index = 0; index < deltas.Length; index++)
            {
                Vector2Int currentPos = from;
                Vector2Int delta = deltas[index];
                if (Math.Sign(offset.X) != Math.Sign(delta.X) || Math.Sign(offset.Y) != Math.Sign(delta.Y))
                    continue;
                for (int mult = 0; mult < multiplier; mult++)
                {
                    currentPos += offset;
                    if (currentPos == targets[index])
                    {
                        // Hit target square (regardless of piece existing there or not)
                        return true;
                    }
                    if (!currentPos.Inside(boardWidth, boardHeight))
                        break;

                    Piece onSquare = squares[currentPos.X, currentPos.Y];
                    if (onSquare != null)
                    {
                        break;
                    }
                }
            }



        }

        return false;
    }
    
    public Vector2Int[] GetOffsets() => offsets;
    public int GetMultiplier() => multiplier;

    public static SlidingMovement Knight => new(Vector2Int.KnightHops, 1);
    public static SlidingMovement Bishop => new(Vector2Int.Diagonals, 7);
    public static SlidingMovement Rook => new(Vector2Int.Cardinals, 7);
    public static SlidingMovement Queen => new(Vector2Int.AllDirections, 7);
    public static SlidingMovement King => new(Vector2Int.AllDirections, 1);

}
