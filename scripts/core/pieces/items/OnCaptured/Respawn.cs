using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;

/// <summary>
/// When this piece is captured, it instead respawns on it's starting location (once, and only if that location is empty)
/// </summary>
/// <param name="pieceId"></param>
public class Respawn(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURED)
{
    public override bool ConditionsMet(Board board, Move move)
    {
        // Find original position for this piece (first board, pieceId, position)
        // If position currently not free, return false
        
        Vector2Int respawnPos = GetRootPosition(board);
        if (board.Squares[respawnPos.X, respawnPos.Y] is not null)
            return false;
        
        return true;
    }

    public override Board Execute(Board board, Move move)
    {
        // Get piece from last board (it's been captured in this one)
        Vector2Int respawnPos = GetRootPosition(board);
        Piece toRespawn = GetPiece(board.LastBoard);
        if (toRespawn is null)
            return board;
        
        toRespawn = new Piece(PieceId, toRespawn.BasePiece, toRespawn.Color, respawnPos, toRespawn.Movement, toRespawn.SpecialPieceType);
        
        Piece[] newPieces = new Piece[board.Pieces.Length + 1];
        for (int i = 0; i < board.Pieces.Length; i++)
        {
            newPieces[i] = board.Pieces[i];
        }
        newPieces[^1] = toRespawn;
        board.Pieces = newPieces;
        board.Squares[respawnPos.X, respawnPos.Y] = toRespawn;
        
        // Remove the respawn item from the dict
        Dictionary<byte, IItem[]> newItems = new();
        foreach (KeyValuePair<byte, IItem[]> pieceToItem in board.ItemsPerPiece)
        {
            if (pieceToItem.Key == PieceId)
            {
                // Copy over all items but remove self
                IItem[] newItemsForPiece = new IItem[pieceToItem.Value.Length - 1];
                int index = 0;
                foreach (IItem item in pieceToItem.Value)
                {
                    if (item == this)
                        continue;
                    newItemsForPiece[index] = item;
                    index++;
                }
                newItems[pieceToItem.Key] = newItemsForPiece;
                continue;
            }
            // Unaffected piece, copy over and continue
            newItems[pieceToItem.Key] = pieceToItem.Value;
        }
        board.ItemsPerPiece = newItems;

        return board;
    }

    private Piece GetPiece(Board board)
    {
        Piece toRespawn = null;
        foreach (Piece piece in board.Pieces)
        {
            if (piece.Id != PieceId)
                continue;
            
            toRespawn = piece;
        }
        return toRespawn;
    }

    private Vector2Int GetRootPosition(Board board)
    {
        // Get root board
        Board rootBoard = board;
        while (rootBoard.LastBoard is not null)
        {
            rootBoard = rootBoard.LastBoard;
        }
        foreach (Piece piece in rootBoard.Pieces)
        {
            if (piece.Id != PieceId)
                continue;
            return piece.Position;
        }
        throw new KeyNotFoundException($"Piece with id {PieceId} not found at root board");
    }
}