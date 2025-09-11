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
        if (forwardPos.Inside(width, height) && board.Squares.Get(forwardPos) is null)
        {
            Move forward = new(id, from, forwardPos, board);
            
            forward.ApplyEvent(new MovePieceEvent(id, from, forwardPos));
            forward.ApplyEvent(new NextTurnEvent());
            
            options.Add(forward);
            
            // Double push (if possible)
            bool onDoubleMoveRow = (color && from.Y == 1) || (!color && from.Y == 6);
            if (onDoubleMoveRow)
            {
                Vector2Int doubleMovePos = forwardPos + direction;
                if (board.Squares.Get(doubleMovePos) is null)
                {
                    Move doubleMove = new(id, from, doubleMovePos, board);
                    doubleMove.ApplyEvent(new ChangePieceTypeEvent(id, SpecialPieceTypes.EN_PASSANTABLE_PAWN));
                    doubleMove.ApplyEvent(new MovePieceEvent(id, from, doubleMovePos));
                    doubleMove.ApplyEvent(new NextTurnEvent());
                    
                    options.Add(doubleMove);
                }
            }
        }
        
        // Capture left
        Vector2Int capLeftPos = forwardPos + Vector2Int.Left;
        if (capLeftPos.Inside(width, height))
        {
            Piece toCapture = board.Squares.Get(capLeftPos);
            
            // En passant, if possible
            Vector2Int enPassantPos = from + Vector2Int.Left;
            Piece enPassant = board.Squares.Get(enPassantPos);
            if (toCapture is null && enPassant is not null && enPassant.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
                toCapture = enPassant;
            
            if (toCapture is not null && toCapture.Color != color)
            {
                Move capLeftMove = new(id, from, capLeftPos, board);
                capLeftMove.ApplyEvent(new MovePieceEvent(id, from, capLeftPos));
                capLeftMove.ApplyEvent(new CapturePieceEvent(toCapture.Id, id));
                capLeftMove.ApplyEvent(new NextTurnEvent());
                
                options.Add(capLeftMove);
            }
        }
        
        // Capture right
        Vector2Int capRightPos = forwardPos + Vector2Int.Right;
        if (capRightPos.Inside(width, height))
        {
            Piece toCapture = board.Squares.Get(capRightPos);
            
            // En passant, if possible
            Vector2Int enPassantPos = from + Vector2Int.Right;
            Piece enPassant = board.Squares.Get(enPassantPos);
            if (toCapture is null && enPassant is not null && enPassant.SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN)
                toCapture = enPassant;
            
            if (toCapture is not null && toCapture.Color != color)
            {
                Move capRightMove = new(id, from, capRightPos, board);
                
                capRightMove.ApplyEvent(new MovePieceEvent(id, from, capRightPos));
                capRightMove.ApplyEvent(new CapturePieceEvent(toCapture.Id, id));
                capRightMove.ApplyEvent(new NextTurnEvent());
                
                options.Add(capRightMove);
            }
        }

        return options;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
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

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
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

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "PAWN";
    }
}
