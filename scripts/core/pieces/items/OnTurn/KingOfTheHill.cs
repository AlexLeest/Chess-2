using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot.Collections;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs.OnTurn;

/// <summary>
/// If piece is in centre of the board, "evolve" it to a stronger piece
/// PAWN -> KNIGHT -> BISHOP -> ROOK -> QUEEN -> KING??
/// </summary>
/// <param name="pieceId"></param>
public class KingOfTheHill(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_TURN)
{
    private readonly System.Collections.Generic.Dictionary<BasePiece, BasePiece> evolutionSteps = new()
    {
        { BasePiece.PAWN, BasePiece.KNIGHT },
        { BasePiece.KNIGHT, BasePiece.BISHOP },
        { BasePiece.BISHOP, BasePiece.ROOK },
        { BasePiece.ROOK, BasePiece.QUEEN },
        // { BasePiece.QUEEN, BasePiece.KING }
    };

    public override bool ConditionsMet(Board board, Move move)
    {
        if (move.Moving == PieceId)
            return false;
        
        Piece piece = board.GetPiece(PieceId);
        if (piece?.Position.X is >= 3 and <= 4 && piece.Position.Y is >= 3 and <= 4)
            return true;
        
        return false;
    }

    public override Board Execute(Board board, Move move)
    {
        Piece piece = board.GetPiece(PieceId);
        if (evolutionSteps.TryGetValue(piece.BasePiece, out BasePiece nextBasePiece))
        {
            // Change BasePiece type to the next one, change the first movement entry out for the default of the next one as well
            piece.BasePiece = nextBasePiece;
            
            // Have to copy the movement array over because it's passed by reference and editing spot 0 changes it for every board
            IMovement[] newMovement = new IMovement[piece.Movement.Length];
            newMovement[0] = DefaultMovements.Get(nextBasePiece);
            for (int i = 1; i < piece.Movement.Length; i++)
                newMovement[i] = piece.Movement[i];
            piece.Movement = newMovement;
        }
        return board;
    }
}
