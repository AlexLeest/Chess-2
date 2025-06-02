using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class CastlingMovement : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        List<Move> result = [];
        bool colorToMove = board.ColorToMove;

        int colorIndex = colorToMove ? 0 : 1;
        int rank = colorToMove ? 0 : 7;
        if (board.CastleKingSide[colorIndex])
        {
            Vector2Int[] checkPositions = [new(4, rank), new(5, rank), new(6, rank)];
            if (!board.IsInCheck(color, checkPositions))
            {
                result.Add(new Move(id, from, new Vector2Int(6, rank), null, SpecialMoveFlag.CASTLE_KINGSIDE));
            }
        }
        if (board.CastleQueenSide[colorIndex])
        {
            Vector2Int[] checkPositions = [new(4, rank), new(3, rank), new(2, rank)];
            if (!board.IsInCheck(color, checkPositions))
            {
                result.Add(new Move(id, from, new Vector2Int(2, rank), null, SpecialMoveFlag.CASTLE_QUEENSIDE));
            }
        }

        return result;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        return false;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        return false;
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "CASTLING";
    }
}
