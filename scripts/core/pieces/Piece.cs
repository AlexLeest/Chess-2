using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class Piece(byte id, BasePiece basePiece, bool color, Vector2Int position, IMovement[] movement, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
{
    public byte Id = id;
    public BasePiece BasePiece = basePiece;
    
    public bool Color = color;
    public Vector2Int Position = position;
    public IMovement[] Movement = movement;
    public SpecialPieceTypes SpecialPieceType = specialPiece;

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
        return new Piece(id, BasePiece.Pawn, color, position, [new PawnMovement()], SpecialPieceTypes.PAWN);
    }

    public static Piece Knight(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.Knight, color, position, [SlidingMovement.Knight], specialPiece);
    }

    public static Piece Bishop(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.Bishop, color, position, [SlidingMovement.Bishop], specialPiece);
    }

    public static Piece Rook(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.Rook, color, position, [SlidingMovement.Rook], specialPiece);
    }

    public static Piece Queen(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.Queen, color, position, [SlidingMovement.Queen], specialPiece);
    }

    public static Piece King(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.KING)
    {
        return new Piece(id, BasePiece.King, color, position, [SlidingMovement.King], specialPiece);
    }
}
