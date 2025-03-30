using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class PawnMovement : IMovement
{

    public List<Vector2Int> GetMovementOptions(Vector2Int from, Piece[,] board, bool color)
    {
        int boardWidth = board.GetLength(0);
        int boardHeight = board.GetLength(1);
        
        List<Vector2Int> options = [];
        Vector2Int direction = new(0, (color ? 1 : -1));

        // Step forward
        Vector2Int forward = from + direction;
        if (forward.Inside(boardWidth, boardHeight) && board[forward.X, forward.Y] is null)
            options.Add(forward);
        
        // Double move forward
        bool onDoubleMoveRow = (color && from.Y == 1) || (!color && from.Y == 6);
        if (onDoubleMoveRow)
        {
            Vector2Int doubleMove = forward + direction;
            if (board[doubleMove.X, doubleMove.Y] is null)
                options.Add(doubleMove);
        }
        // Captures
        Vector2Int captureLeft = new(forward.X - 1, forward.Y);
        if (captureLeft.Inside(boardWidth, boardHeight))
        {
            Piece toTake = board[captureLeft.X, captureLeft.Y];
            if (toTake is not null && toTake.Color != color)
            {
                options.Add(captureLeft);
            }
        }
        Vector2Int captureRight = new(forward.X + 1, forward.Y);
        if (captureRight.Inside(boardWidth, boardHeight))
        {
            Piece toTake = board[captureRight.X, captureRight.Y];
            if (toTake is not null && toTake.Color != color)
            {
                options.Add(captureRight);
            }
        }

        return options;
    }
}
