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

    public uint GetZobristHash()
    {
        // Zobrist hash for piece is the zobrist hash for every IMovement it has XORed
        // XOR BasePiece hash
        // XOR SpecialPieceTypes hash
        uint result = 0;
        foreach (IMovement movement in Movement)
        {
            result ^= movement.GetZobristHash(color, Position);
        }
        result ^= ZobristCalculator.GetZobristHash(color, Position, BasePiece);
        result ^= ZobristCalculator.GetZobristHash(color, Position, SpecialPieceType);

        return result;
    }

    public uint GetZobristHash(Vector2Int position)
    {
        // Zobrist hash for piece is the zobrist hash for every IMovement it has XORed
        // XOR BasePiece hash
        // XOR SpecialPieceTypes hash
        uint result = 0;
        foreach (IMovement movement in Movement)
        {
            result ^= movement.GetZobristHash(color, position);
        }
        result ^= ZobristCalculator.GetZobristHash(color, position, BasePiece);
        result ^= ZobristCalculator.GetZobristHash(color, position, SpecialPieceType);

        return result;
    }

    public override int GetHashCode()
    {
        // Possible overflow? Should I worry abt this?
        return (int)GetZobristHash();
    }

    private static readonly char[] files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
    public override string ToString()
    {
        string color = Color ? "WHITE" : "BLACK";
        return $"{files[Position.X]}{Position.Y + 1}, {color}, movement: {Movement[0]}";
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
        return new Piece(id, BasePiece.KING, color, position, [SlidingMovement.King, new CastlingMovement()], specialPiece);
    }
}
