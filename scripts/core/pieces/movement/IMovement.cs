using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

// TODO: Can these all be structs? (optimization stuff)
public interface IMovement
{
    public List<Vector2Int> GetMovementOptions(Vector2Int from, Piece[,] squares, bool color);
}