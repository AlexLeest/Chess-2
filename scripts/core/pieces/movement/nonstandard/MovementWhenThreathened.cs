using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

/// <summary>
/// Movement option that only becomes available when the piece is threatened
/// </summary>
/// <param name="baseMovement">The movement in question</param>
public class MovementWhenThreathened(IMovement baseMovement) : IMovement
{
    private IMovement baseMovement = baseMovement;

    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        if (!board.IsInCheck(color, from))
            return [];
        
        return baseMovement.GetMovementOptions(id, from, board, color);
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        if (!board.IsInCheck(color, from))
            return false;
        
        return baseMovement.Attacks(from, target, board, color);
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        if (!board.IsInCheck(color, from))
            return false;

        return baseMovement.AttacksAny(from, targets, board, color);
    }

    public int GetZobristHash(bool color, Vector2Int position)
    {
        // TODO: Non standard zobrist hashing.
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return $"WHEN THREATENED: {baseMovement}";
    }
}
