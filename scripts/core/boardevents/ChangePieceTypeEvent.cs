using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class ChangePieceTypeEvent(byte pieceId, SpecialPieceTypes type) : IBoardEvent
{
    public readonly byte PieceId = pieceId;
    public readonly SpecialPieceTypes Type = type;
    
    public void AdjustBoard(Board board, Move move)
    {
        Piece piece = board.GetPiece(PieceId);
        
        // XOR out the old type, XOR in the new type
        board.ZobristHash ^= ZobristCalculator.GetZobristHash(piece.Color, piece.Position, piece.SpecialPieceType);
        piece.SpecialPieceType = Type;
        board.ZobristHash ^= ZobristCalculator.GetZobristHash(piece.Color, piece.Position, Type);
    }
}
