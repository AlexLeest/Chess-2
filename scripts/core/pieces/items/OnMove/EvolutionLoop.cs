using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;

/// <summary>
/// This piece "evolves" into the next type of piece every time it moves
/// PAWN -> KNIGHT -> BISHOP -> ROOK -> QUEEN -> PAWN
/// </summary>
/// <param name="pieceId"></param>
public class EvolutionLoop(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_MOVE)
{
    private readonly Dictionary<BasePiece, BasePiece> evolutionSteps = new()
    {
        { BasePiece.PAWN, BasePiece.KNIGHT },
        { BasePiece.KNIGHT, BasePiece.BISHOP },
        { BasePiece.BISHOP, BasePiece.ROOK },
        { BasePiece.ROOK, BasePiece.QUEEN },
        { BasePiece.QUEEN, BasePiece.PAWN },
        // { BasePiece.QUEEN ,BasePiece.KING},
        // { BasePiece.KING , BasePiece.PAWN},
    };

    public override Board Execute(Board board, Move move)
    {
        Piece moved = board.GetPiece(PieceId);
        if (evolutionSteps.TryGetValue(moved.BasePiece, out BasePiece nextBasePiece))
        {
            // Change BasePiece type to the next one, change the first movement entry out for the default of the next one as well
            moved.BasePiece = nextBasePiece;
            
            // Have to copy the movement array over because it's passed by reference and editing spot 0 changes it for every board
            IMovement[] newMovement = new IMovement[moved.Movement.Length];
            newMovement[0] = DefaultMovements.Get(nextBasePiece);
            for (int i = 1; i < moved.Movement.Length; i++)
                newMovement[i] = moved.Movement[i];
            moved.Movement = newMovement;
        }
        return board;
    }
}
