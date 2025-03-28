using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public interface IMovement
{
    public List<Vector2Int> GetMovementOptions(Vector2Int from, Piece[,] board, bool color);
}