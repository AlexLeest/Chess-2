namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class RemoveMovementEvent(byte pieceId, IMovement toRemove) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece piece = board.GetPiece(pieceId);

        IMovement[] newMovements = new IMovement[piece.Movement.Length - 1];
        int index = 0;
        foreach (IMovement movement in piece.Movement)
        {
            if (movement == toRemove)
                continue;
            newMovements[index] = movement;
            index++;
        }
        piece.Movement = newMovements;
        
        // XOR out movement hash
        board.ZobristHash ^= toRemove.GetZobristHash(piece.Color, piece.Position);
    }
}
