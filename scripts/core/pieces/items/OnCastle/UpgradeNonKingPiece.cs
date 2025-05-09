using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCastle;

public class UpgradeNonKingPiece(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CASTLE) 
{
    private readonly System.Collections.Generic.Dictionary<BasePiece, BasePiece> evolutionSteps = new()
    {
        { BasePiece.PAWN, BasePiece.KNIGHT },
        { BasePiece.KNIGHT, BasePiece.BISHOP },
        { BasePiece.BISHOP, BasePiece.ROOK },
        { BasePiece.ROOK, BasePiece.QUEEN },
        // { BasePiece.QUEEN, BasePiece.KING }
    };

    public override Board Execute(Board board, Move move)
    {
        // Get non-king piece from castling move
        int xCoord = move.To.X;
        
        Piece nonKingPiece = xCoord == 2 ? board.Squares[3, move.To.Y] : board.Squares[5, move.To.Y];
        nonKingPiece.Upgrade();
        
        return board;
    }
}
