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
        
        if (evolutionSteps.TryGetValue(nonKingPiece.BasePiece, out BasePiece nextBasePiece))
        {
            // Change BasePiece type to the next one, change the first movement entry out for the default of the next one as well
            nonKingPiece.BasePiece = nextBasePiece;
            
            // Have to copy the movement array over because it's passed by reference and editing spot 0 changes it for every board
            IMovement[] newMovement = new IMovement[nonKingPiece.Movement.Length];
            newMovement[0] = DefaultMovements.Get(nextBasePiece);
            for (int i = 1; i < nonKingPiece.Movement.Length; i++)
                newMovement[i] = nonKingPiece.Movement[i];
            nonKingPiece.Movement = newMovement;
        }
        return board;
    }
}
