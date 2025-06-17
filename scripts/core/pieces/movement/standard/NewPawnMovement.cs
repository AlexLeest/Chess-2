using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class NewPawnMovement : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        List<Move> result = [];
        Vector2Int direction = new(0, (color ? 1 : -1));

        int width = board.Squares.GetLength(0);
        int height = board.Squares.GetLength(1);
        
        // Push forward
        Vector2Int forwardPos = from + direction;
        if (forwardPos.Inside(width, height) && board.Squares[forwardPos.X, forwardPos.Y] is null)
        {
            Move forward = new Move(id, from, forwardPos, board);
            MovePieceEvent push = new MovePieceEvent(id, forwardPos);
            forward.ApplyEvent(push);
            result.Add(forward);
            
            // Double push (if possible)
            bool onDoubleMoveRow = (color && from.Y == 1) || (!color && from.Y == 6);
            if (onDoubleMoveRow)
            {
                Vector2Int doubleMovePos = forwardPos + direction;
                if (board.Squares[doubleMovePos.X, doubleMovePos.Y] is null)
                {
                    Move doubleMove = new Move(id, from, doubleMovePos, board);
                    MovePieceEvent doublePush = new MovePieceEvent(id, doubleMovePos);
                    doubleMove.ApplyEvent(doublePush);
                    result.Add(doubleMove);
                }
            }
        }
        

        return result;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        throw new System.NotImplementedException();
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        throw new System.NotImplementedException();
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        throw new System.NotImplementedException();
    }
}
