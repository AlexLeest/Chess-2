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

    public IEnumerable<Move> GetMovementOptions(Board board)
    {
        foreach (IMovement movement in Movement)
            foreach (Move move in movement.GetMovementOptions(Id, Position, board, Color))
                yield return move;
    }

    public Piece DeepCopy()
    {
        // En-passant decay, otherwise copy everything
        return new Piece(Id, BasePiece, Color, Position, Movement, SpecialPieceType == SpecialPieceTypes.EN_PASSANTABLE_PAWN ? SpecialPieceTypes.PAWN : SpecialPieceType);
    }

    public static Piece Pawn(byte id, bool color, Vector2Int position)
    {
        return new Piece(id, BasePiece.PAWN, color, position, [new PawnMovement()], SpecialPieceTypes.PAWN);
    }

    public static Piece Knight(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.KNIGHT, color, position, [SlidingMovement.Knight], specialPiece);
    }

    public static Piece Bishop(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.BISHOP, color, position, [SlidingMovement.Bishop], specialPiece);
    }

    public static Piece Rook(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.ROOK, color, position, [SlidingMovement.Rook], specialPiece);
    }

    public static Piece Queen(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.NONE)
    {
        return new Piece(id, BasePiece.QUEEN, color, position, [SlidingMovement.Queen], specialPiece);
    }

    public static Piece King(byte id, bool color, Vector2Int position, SpecialPieceTypes specialPiece = SpecialPieceTypes.KING)
    {
        return new Piece(id, BasePiece.KING, color, position, [SlidingMovement.King], specialPiece);
    }
}
