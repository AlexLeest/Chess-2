using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
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
        // TODO: Handle promotion (here or in MovePieceEvent?)
        // TODO: Add making a pawn en-passantable
        
        List<Move> options = [];
        Vector2Int direction = new(0, (color ? 1 : -1));

        int width = board.Squares.GetLength(0);
        int height = board.Squares.GetLength(1);
        
        // Push forward
        Vector2Int forwardPos = from + direction;
        if (forwardPos.Inside(width, height) && board.Squares[forwardPos.X, forwardPos.Y] is null)
        {
            Move forward = new(id, from, forwardPos, board);
            
            // MovePieceEvent push = new(id, forwardPos);
            forward.ApplyEvent(new MovePieceEvent(id, forwardPos));
            forward.ApplyEvent(new NextTurnEvent());
            
            options.Add(forward);
            
            // Double push (if possible)
            bool onDoubleMoveRow = (color && from.Y == 1) || (!color && from.Y == 6);
            if (onDoubleMoveRow)
            {
                Vector2Int doubleMovePos = forwardPos + direction;
                if (board.Squares[doubleMovePos.X, doubleMovePos.Y] is null)
                {
                    Move doubleMove = new(id, from, doubleMovePos, board);
                    // MovePieceEvent doublePush = new(id, doubleMovePos);
                    doubleMove.ApplyEvent(new MovePieceEvent(id, doubleMovePos));
                    doubleMove.ApplyEvent(new ChangePieceTypeEvent(id, SpecialPieceTypes.EN_PASSANTABLE_PAWN));
                    doubleMove.ApplyEvent(new NextTurnEvent());
                    
                    options.Add(doubleMove);
                }
            }
        }
        
        // Capture left
        Vector2Int capLeftPos = forwardPos + Vector2Int.Left;
        // if (capLeftPos.Inside(width, height) && board.Squares[capLeftPos.X, capLeftPos.Y] is not null)
        if (capLeftPos.Inside(width, height))
        {
            Piece toCapture = board.Squares[capLeftPos.X, capLeftPos.Y];
            
            // En passant, if possible
            Vector2Int enPassantPos = from + Vector2Int.Left;
            Piece enPassant = board.Squares[enPassantPos.X, enPassantPos.Y];
            if (toCapture is null && enPassant is not null && enPassant.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
                toCapture = enPassant;
            
            if (toCapture is not null && toCapture.Color != color)
            {
                Move capLeftMove = new(id, from, capLeftPos, board);
                capLeftMove.ApplyEvent(new MovePieceEvent(id, capLeftPos));
                capLeftMove.ApplyEvent(new CapturePieceEvent(toCapture.Id, id));
                capLeftMove.ApplyEvent(new NextTurnEvent());
                
                options.Add(capLeftMove);
            }
        }
        
        // Capture right
        Vector2Int capRightPos = forwardPos + Vector2Int.Right;
        // if (capRightPos.Inside(width, height) && board.Squares[capRightPos.X, capRightPos.Y] is not null)
        if (capRightPos.Inside(width, height))
        {
            Piece toCapture = board.Squares[capRightPos.X, capRightPos.Y];
            
            // En passant, if possible
            Vector2Int enPassantPos = from + Vector2Int.Right;
            Piece enPassant = board.Squares[enPassantPos.X, enPassantPos.Y];
            if (toCapture is null && enPassant is not null && enPassant.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
                toCapture = enPassant;
            
            if (toCapture is not null && toCapture.Color != color)
            {
                Move capRightMove = new(id, from, capRightPos, board);
                
                capRightMove.ApplyEvent(new MovePieceEvent(id, capRightPos));
                capRightMove.ApplyEvent(new CapturePieceEvent(toCapture.Id, id));
                capRightMove.ApplyEvent(new NextTurnEvent());
                
                options.Add(capRightMove);
            }
        }

        return options;
    }
    // public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    // {
    //     Piece[,] squares = board.Squares;
    //     SpecialMoveFlag moveFlag = from.Y == (color ? 6 : 1) ? SpecialMoveFlag.PROMOTION : SpecialMoveFlag.NONE;
    //     
    //     int boardWidth = squares.GetLength(0);
    //     int boardHeight = squares.GetLength(1);
    //     
    //     Vector2Int direction = new(0, (color ? 1 : -1));
    //     List<Move> options = [];
    //
    //     // Step forward
    //     Vector2Int forward = from + direction;
    //     if (forward.Inside(boardWidth, boardHeight) && squares[forward.X, forward.Y] is null)
    //     {
    //         options.Add(new Move(id, from, forward, null, moveFlag));
    //         // Double move forward
    //         bool onDoubleMoveRow = (color && from.Y == 1) || (!color && from.Y == 6);
    //         if (onDoubleMoveRow)
    //         {
    //             Vector2Int doubleMove = forward + direction;
    //             if (squares[doubleMove.X, doubleMove.Y] is null)
    //                 options.Add(new Move(id, from, doubleMove));
    //         }
    //     }
    //     
    //     // Captures
    //     Vector2Int upLeft = new(forward.X - 1, forward.Y);
    //     Piece captureLeft = AttemptCapture(squares, color, upLeft);
    //     if (captureLeft is not null)
    //         options.Add(new Move(id, from, upLeft, captureLeft, moveFlag));
    //     
    //     Vector2Int upRight = new(forward.X + 1, forward.Y);
    //     Piece captureRight = AttemptCapture(squares, color, upRight);
    //     if (captureRight is not null)
    //         options.Add(new Move(id, from, upRight, captureRight, moveFlag));
    //     
    //     // En passant checks
    //     Vector2Int enPassantLeft = new(from.X - 1, from.Y);
    //     if (enPassantLeft.Inside(boardWidth, boardHeight))
    //     {
    //         Piece toTake = squares[enPassantLeft.X, enPassantLeft.Y];
    //         if (toTake is not null && toTake.Color != color && toTake.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
    //         {
    //             options.Add(new Move(id, from, enPassantLeft + direction, toTake));
    //         }
    //     }
    //     Vector2Int enPassantRight = new(from.X + 1, from.Y);
    //     if (enPassantRight.Inside(boardWidth, boardHeight))
    //     {
    //         Piece toTake = squares[enPassantRight.X, enPassantLeft.Y];
    //         if (toTake is not null && toTake.Color != color && toTake.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
    //         {
    //             options.Add(new Move(id, from, enPassantRight + direction, toTake));
    //         }
    //     }
    //
    //     return options;
    // }
    //
    // private static Piece AttemptCapture(Piece[,] squares, bool color, Vector2Int capturePos)
    // {
    //     if (!capturePos.Inside(squares.GetLength(0), squares.GetLength(1)))
    //         return null;
    //     
    //     Piece toTake = squares[capturePos.X, capturePos.Y];
    //     if (toTake is not null && toTake.Color != color)
    //     {
    //         return toTake;
    //     }
    //     return null;
    // }

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

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "PAWN";
    }
}
