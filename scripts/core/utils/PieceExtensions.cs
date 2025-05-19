using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class PieceExtensions
{
    private static readonly Dictionary<BasePiece, BasePiece> evolutionSteps = new()
    {
        { BasePiece.PAWN, BasePiece.KNIGHT },
        { BasePiece.KNIGHT, BasePiece.BISHOP },
        { BasePiece.BISHOP, BasePiece.ROOK },
        { BasePiece.ROOK, BasePiece.QUEEN },
        // { BasePiece.QUEEN, BasePiece.KING }
    };
    
    public static Piece Upgrade(this Piece piece)
    {
        if (evolutionSteps.TryGetValue(piece.BasePiece, out BasePiece nextBasePiece))
        {
            // Change BasePiece type to the next one, change the first movement entry out for the default of the next one as well
            piece.BasePiece = nextBasePiece;
            piece.SpecialPieceType = nextBasePiece == BasePiece.PAWN ? SpecialPieceTypes.PAWN : SpecialPieceTypes.NONE;
            
            // Have to copy the movement array over because it's passed by reference and editing spot 0 changes it for every board
            IMovement[] newMovement = new IMovement[piece.Movement.Length];
            newMovement[0] = DefaultMovements.Get(nextBasePiece);
            for (int i = 1; i < piece.Movement.Length; i++)
                newMovement[i] = piece.Movement[i];
            piece.Movement = newMovement;
        }

        return piece;
    }

    public static Piece ChangeTo(this Piece piece, BasePiece newBasePiece)
    {
        // Change BasePiece type to the next one, change the first movement entry out for the default of the next one as well
        piece.BasePiece = newBasePiece;
        piece.SpecialPieceType = newBasePiece == BasePiece.PAWN ? SpecialPieceTypes.PAWN : SpecialPieceTypes.NONE;
            
        // Have to copy the movement array over because it's passed by reference and editing spot 0 changes it for every board
        IMovement[] newMovement = new IMovement[piece.Movement.Length];
        newMovement[0] = DefaultMovements.Get(newBasePiece);
        for (int i = 1; i < piece.Movement.Length; i++)
            newMovement[i] = piece.Movement[i];
        piece.Movement = newMovement;

        return piece;
    }
}
