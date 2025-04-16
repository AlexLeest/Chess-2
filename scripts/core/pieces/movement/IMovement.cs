using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

// TODO: Can these all be structs? (optimization stuff)
public interface IMovement
{
    public List<Vector2Int> GetMovementOptions(Vector2Int from, Piece[,] squares, bool color);
    
    public bool Attacks(Vector2Int from, Vector2Int to, Piece[,] squares, bool color);
}