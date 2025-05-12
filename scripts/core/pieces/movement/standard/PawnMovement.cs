using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

/// <summary>
/// Regular pawn movement
/// </summary>
public class PawnMovement : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        Piece[,] squares = board.Squares;
        
        int boardWidth = squares.GetLength(0);
        int boardHeight = squares.GetLength(1);
        
        Vector2Int direction = new(0, (color ? 1 : -1));
        List<Move> options = [];

        // Step forward
        Vector2Int forward = from + direction;
        if (forward.Inside(boardWidth, boardHeight) && squares[forward.X, forward.Y] is null)
        {
            options.Add(new Move(id, from, forward));
            // Double move forward
            bool onDoubleMoveRow = (color && from.Y == 1) || (!color && from.Y == 6);
            if (onDoubleMoveRow)
            {
                Vector2Int doubleMove = forward + direction;
                if (squares[doubleMove.X, doubleMove.Y] is null)
                    options.Add(new Move(id, from, doubleMove));
            }
        }
        
        // Captures
        Vector2Int upLeft = new Vector2Int(forward.X - 1, forward.Y);
        Piece captureLeft = AttemptCapture(squares, color, upLeft);
        if (captureLeft is not null)
            options.Add(new Move(id, from, upLeft, captureLeft));
        
        Vector2Int upRight = new Vector2Int(forward.X + 1, forward.Y);
        Piece captureRight = AttemptCapture(squares, color, upRight);
        if (captureRight is not null)
            options.Add(new Move(id, from, upRight, captureRight));
        
        // En passant checks
        // TODO: Does not currently handle actually CAPTURING that pawn
        Vector2Int enPassantLeft = new(from.X - 1, from.Y);
        if (enPassantLeft.Inside(boardWidth, boardHeight))
        {
            Piece toTake = squares[enPassantLeft.X, enPassantLeft.Y];
            if (toTake is not null && toTake.Color != color && toTake.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
            {
                options.Add(new Move(id, from, enPassantLeft + direction, toTake));
            }
        }
        Vector2Int enPassantRight = new(from.X + 1, from.Y);
        if (enPassantRight.Inside(boardWidth, boardHeight))
        {
            Piece toTake = squares[enPassantRight.X, enPassantLeft.Y];
            if (toTake is not null && toTake.Color != color && toTake.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
            {
                options.Add(new Move(id, from, enPassantRight + direction, toTake));
            }
        }

        return options;
    }

    private static Piece AttemptCapture(Piece[,] squares, bool color, Vector2Int capturePos)
    {
        if (!capturePos.Inside(squares.GetLength(0), squares.GetLength(1)))
            return null;
        
        Piece toTake = squares[capturePos.X, capturePos.Y];
        if (toTake is not null && toTake.Color != color)
        {
            return toTake;
        }
        return null;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        Piece[,] squares = board.Squares;

        // Pawns can only attack diagonal
        int xOffset = from.X - target.X;
        if (xOffset != 1 && xOffset != -1)
            return false;
        
        int directionY = color ? 1 : -1;
        if (from.Y - target.Y != directionY)
            return false;

        return true;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        Piece[,] squares = board.Squares;

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

    public override string ToString()
    {
        return "PAWN";
    }
}
