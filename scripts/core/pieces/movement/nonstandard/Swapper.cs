using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.movement.nonstandard;

/// <summary>
/// Allows the piece to swap spots with other pieces of the same color
/// </summary>
public class Swapper : IMovement
{

    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        List<Move> options = [];
        foreach (Piece piece in board.Pieces)
        {
            // Can only swap with pieces of the same color, shouldn't be allowed to swap with itself
            if (piece.Color != color || piece.Id == id)
                continue;
            
            // Swap the two pieces
            Move move = new Move(id, from, piece.Position, board);
            move.ApplyEvent(new MovePieceEvent(id, from, piece.Position));
            move.ApplyEvent(new MovePieceEvent(piece.Id, piece.Position, from));
            move.ApplyEvent(new NextTurnEvent());

            options.Add(move);
        }
        return options;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        // Never attacks anything
        return false;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        // Never attacks anything
        return false;
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "SWAPPER";
    }
}
