using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;

/// <summary>
/// When this piece is captured, it instead respawns on it's starting location (once, and only if that location is empty)
/// </summary>
/// <param name="pieceId"></param>
public class Respawn(byte pieceId) : AbstractItem(pieceId, ItemTriggers.AFTER_CAPTURED)
{
    public override bool ConditionsMet(Board board, Move move, IBoardEvent trigger)
    {
        // Find original position for this piece (first board, pieceId, position)
        // If position currently not free, return false
        
        Vector2Int respawnPos = GetRootPosition(board);
        if (move.Result.Squares.Get(respawnPos) is not null)
            return false;
        
        return true;
    }

    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        // Get piece from last board (it's been captured in this one)
        Vector2Int respawnPos = GetRootPosition(board);
        Piece toRespawn = board.LastBoard.GetPiece(PieceId).DeepCopy(false);
        if (toRespawn is null)
            return board;
        
        toRespawn = new Piece(PieceId, toRespawn.BasePiece, toRespawn.Color, respawnPos, toRespawn.Movement, toRespawn.SpecialPieceType);
        move.ApplyEvent(new SpawnPieceEvent(toRespawn));
        move.ApplyEvent(new RemoveItemEvent(toRespawn.Id, this));

        return board;
    }

    private Vector2Int GetRootPosition(Board board)
    {
        // Get root board
        Board rootBoard = board;
        while (rootBoard.LastBoard is not null)
        {
            rootBoard = rootBoard.LastBoard;
        }
        Piece piece = rootBoard.GetPiece(PieceId);
        if (piece is not null)
            return piece.Position;
        
        throw new KeyNotFoundException($"Piece with id {PieceId} not found at root board");
    }
}