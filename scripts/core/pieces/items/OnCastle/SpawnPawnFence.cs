using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCastle;

public class SpawnPawnFence(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CASTLE)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        bool color = board.Turn % 2 == 0;
        // Put 3 pawns in front of the king (if possible)
        int kingX = move.To.X;
        int pawnY = move.To.Y == 0 ? 1 : 6;
        
        byte highestId = board.Pieces.Max(piece => piece.Id);

        for (int xPos = kingX - 1; xPos <= kingX + 1; xPos++)
        {
            Piece onPos = board.Squares[xPos, pawnY];
            if (onPos is not null)
                continue;

            highestId++;
            Vector2Int pawnPos = new(xPos, pawnY);
            Piece newPawn = new(highestId, BasePiece.PAWN, color, pawnPos, [new PawnMovement()], SpecialPieceTypes.PAWN);
            move.ApplyEvent(new SpawnPieceEvent(newPawn));
            // Piece[] newPieces = board.DeepcopyPieces();
            // newPieces[^1] = newPawn;
            // board.Pieces = newPieces;
            // board.Squares.Set(pawnPos, newPawn);
        }

        return board;
    }
}
