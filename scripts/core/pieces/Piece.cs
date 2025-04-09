using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Piece
{
    public byte Id;
    
    public bool Color;
    public Vector2Int Position;
    public IMovement[] Movement;
    public SpecialPieceTypes SpecialPieceType;

    public Piece(byte id, bool color, Vector2Int position, IMovement[] movement, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        Id = id;
        Color = color;
        Position = position;
        Movement = movement;
        SpecialPieceType = specialPiece;
    }

    public IEnumerable<Vector2Int> GetMovementOptions(Piece[,] board)
    {
        foreach (IMovement movement in Movement)
        {
            foreach (Vector2Int move in movement.GetMovementOptions(Position, board, Color))
            {
                yield return move;
            }
        }
    }

    public static Piece Pawn(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, color, position, [new PawnMovement()], SpecialPieceTypes.PAWN);
    }

    public static Piece Knight(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, color, position, [SlidingMovement.Knight], specialPiece);
    }

    public static Piece Bishop(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, color, position, [SlidingMovement.Bishop], specialPiece);
    }

    public static Piece Rook(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, color, position, [SlidingMovement.Rook], specialPiece);
    }

    public static Piece Queen(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, color, position, [SlidingMovement.Queen], specialPiece);
    }

    public static Piece King(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, color, position, [SlidingMovement.King], specialPiece);
    }
}
