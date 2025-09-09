using System;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class AddMovementEvent(byte pieceId, IMovement toAdd) : IBoardEvent
{
    public void AdjustBoard(Board board, Move move)
    {
        Piece piece = board.GetPiece(pieceId);
        IMovement[] newMovement = new IMovement[piece.Movement.Length + 1];
        Array.Copy(piece.Movement, newMovement, piece.Movement.Length);
        newMovement[^1] = toAdd;
        piece.Movement = newMovement;
        
        // XOR in movement hash
        board.ZobristHash ^= toAdd.GetZobristHash(piece.Color, piece.Position);
    }
}
