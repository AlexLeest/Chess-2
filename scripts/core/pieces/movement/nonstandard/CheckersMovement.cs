using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

/// <summary>
/// Checkers style capturing, where you can jump over an opponent piece to capture it
/// </summary>
public class CheckersMovement : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        List<Move> result = [];
        Vector2Int[] diagonals = Vector2Int.Diagonals;

        foreach (Vector2Int direction in diagonals)
        {
            Vector2Int diagCoords = from + direction;
            if (!diagCoords.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
                continue;
            
            Piece diagPiece = board.Squares[diagCoords.X, diagCoords.Y];
            if (diagPiece is null)
            {
                Move move = new(id, from, diagCoords, board);
                MovePieceEvent movePiece = new(id, diagCoords);
                move.ApplyEvent(movePiece);

                result.Add(move);
                continue;
            }
            if (diagPiece.Color == color)
                continue;

            Vector2Int behindPieceCoords = diagCoords + direction;
            if (!behindPieceCoords.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
                continue;
            
            Piece behindPiece = board.Squares[behindPieceCoords.X, behindPieceCoords.Y];
            if (behindPiece is not null)
                continue;
            
            Move capMove = new(id, from, behindPieceCoords, board);
            capMove.ApplyEvent(new MovePieceEvent(id, behindPieceCoords));
            capMove.ApplyEvent(new CapturePieceEvent(diagPiece.Id, id));
            
            result.Add(capMove);
        }

        return result;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        Vector2Int[] diagonals = Vector2Int.Diagonals;
        Vector2Int delta = target - from;
        if (!diagonals.Contains(delta))
            return false;

        Vector2Int behindPieceCoords = target + delta;
        if (!behindPieceCoords.Inside(board.Squares.GetLength(0), board.Squares.GetLength(1)))
            return false;
        
        Piece behindPiece = board.Squares[behindPieceCoords.X, behindPieceCoords.Y];
        
        return behindPiece is null;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        foreach (Vector2Int target in targets)
            if (Attacks(from, target, board, color))
                return true;
        return false;
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "CHECKERS";
    }
}
