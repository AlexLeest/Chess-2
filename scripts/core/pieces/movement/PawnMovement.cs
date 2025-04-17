using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

// TODO: Struct, maybe
public class PawnMovement : IMovement
{

    public List<Vector2Int> GetMovementOptions(Vector2Int from, Piece[,] squares, bool color)
    {
        // TODO: En passant
        int boardWidth = squares.GetLength(0);
        int boardHeight = squares.GetLength(1);
        
        List<Vector2Int> options = [];
        Vector2Int direction = new(0, (color ? 1 : -1));

        // Step forward
        Vector2Int forward = from + direction;
        if (forward.Inside(boardWidth, boardHeight) && squares[forward.X, forward.Y] is null)
        {
            options.Add(forward);
            // Double move forward
            bool onDoubleMoveRow = (color && from.Y == 1) || (!color && from.Y == 6);
            if (onDoubleMoveRow)
            {
                Vector2Int doubleMove = forward + direction;
                if (squares[doubleMove.X, doubleMove.Y] is null)
                    options.Add(doubleMove);
            }
        }
        
        // Captures
        AttemptCapture(squares, color, new Vector2Int(forward.X - 1, forward.Y), options);
        AttemptCapture(squares, color, new Vector2Int(forward.X + 1, forward.Y), options);
        
        // En passant checks
        // TODO: Does not currently handle actually CAPTURING that pawn
        Vector2Int enPassantLeft = new(from.X - 1, from.Y);
        if (enPassantLeft.Inside(boardWidth, boardHeight))
        {
            Piece toTake = squares[enPassantLeft.X, enPassantLeft.Y];
            if (toTake is not null && toTake.Color != color && toTake.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
            {
                options.Add(enPassantLeft + direction);
            }
        }
        Vector2Int enPassantRight = new(from.X + 1, from.Y);
        if (enPassantRight.Inside(boardWidth, boardHeight))
        {
            Piece toTake = squares[enPassantRight.X, enPassantLeft.Y];
            if (toTake is not null && toTake.Color != color && toTake.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
            {
                options.Add(enPassantRight + direction);
            }
        }

        return options;
    }

    private static void AttemptCapture(Piece[,] squares, bool color, Vector2Int capturePos, List<Vector2Int> options)
    {
        if (!capturePos.Inside(squares.GetLength(0), squares.GetLength(1)))
            return;
        
        Piece toTake = squares[capturePos.X, capturePos.Y];
        if (toTake is not null && toTake.Color != color)
        {
            options.Add(capturePos);
        }
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Piece[,] squares, bool color)
    {
        // Pawns can only attack diagonal
        int xOffset = from.X - target.X;
        if (xOffset != 1 && xOffset != -1)
            return false;
        
        int directionY = color ? 1 : -1;
        if (from.Y - target.Y != directionY)
            return false;

        return true;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Piece[,] squares, bool color)
    {
        int directionY = color ? 1 : -1;
        foreach (Vector2Int target in targets)
        {
            // Pawns can only attack diagonal
            int xOffset = from.X - target.X;
            if (xOffset != 1 && xOffset != -1)
                continue;
        
            if (from.Y - target.Y != directionY)
                continue;

            return true;
        }
        return false;
    }
}
