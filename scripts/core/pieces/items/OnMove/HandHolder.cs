using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;

public class HandHolder(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_MOVE)
{
    private int boardWidth, boardHeight;
    private Piece piece;
    
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        // Find adjacent pieces with the same base piece type and color, try to move them with you
        piece = board.GetPiece(PieceId);
        if (piece is null)
            return board;
        if (trigger is not MovePieceEvent movePieceEvent)
            return board;
        Vector2Int moveDelta = movePieceEvent.To - move.From;
    
        boardWidth = board.Squares.GetLength(0);
        boardHeight = board.Squares.GetLength(1);
    
        Vector2Int left = move.From + Vector2Int.Left;
        AttemptPieceMove(move, left, moveDelta, trigger);
        Vector2Int right = move.From + Vector2Int.Right;
        AttemptPieceMove(move, right, moveDelta, trigger);
    
        return move.Result;
    }
    
    private void AttemptPieceMove(Move move, Vector2Int from, Vector2Int delta, IBoardEvent trigger)
    {
        // Check if starting and goal location are on board
        Vector2Int goalPos = from + delta;
        if (!from.Inside(boardWidth, boardHeight) || !goalPos.Inside(boardWidth, boardHeight))
            return;
        
        // Check if that location has a piece with the same color and base piece type
        Piece toBeMoved = move.Result.Squares.Get(from);
        if (toBeMoved is null || toBeMoved.BasePiece != piece.BasePiece)
            return;
    
        // ALSO check if that piece wasn't the piece that was moved right before this, to prevent looping behavior
        if (trigger is MovePieceEvent movePieceTrigger && movePieceTrigger.PieceId == toBeMoved.Id)
            return;
    
        move.ApplyEvent(new MovePieceEvent(toBeMoved.Id, from + delta));
    }
}
