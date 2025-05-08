namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnOpponentCastle;

public class CaptureNonKingPiece(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_OPPONENT_CASTLE)
{
    public override Board Execute(Board board, Move move)
    {
        // Get non-king piece in castling, KILL
        int xCoord = move.To.X;
        Piece nonKingPiece;
        if (xCoord == 2)
            nonKingPiece = board.Squares[3, move.To.Y];
        else
            nonKingPiece = board.Squares[5, move.To.Y];
        
        if (nonKingPiece is null)
            return board;

        board.Pieces = board.DeepcopyPieces(nonKingPiece.Id, nonKingPiece.Id);
        return board;
    }
}
