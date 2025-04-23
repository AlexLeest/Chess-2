using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

public class OpponentRoyalSwap(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CASTLE) 
{
    public override bool ConditionsMet(Board board, Move move)
    {
        bool color = GetOwnColor(board);
        foreach (Piece piece in board.Pieces)
        {
            if (piece.BasePiece == BasePiece.QUEEN && piece.Color != color)
                return true;
        }

        return false;
    }

    public override Board Execute(Board board, Move move)
    {
        // Find out own color
        bool color = GetOwnColor(board);
        
        Piece king = null;
        Piece queen = null;
        foreach (Piece piece in board.Pieces)
        {
            if (piece.SpecialPieceType == SpecialPieceTypes.KING && piece.Color != color)
            {
                king = piece;
            }
            else if (piece.BasePiece == BasePiece.QUEEN && piece.Color != color)
            {
                queen = piece;
            }
        }

        if (king is null || queen is null)
        {
            return board;
        }
        Vector2Int kingPos = king.Position;
        Vector2Int queenPos = queen.Position;

        king.Position = queenPos;
        queen.Position = kingPos;
        board.Squares[kingPos.X, kingPos.Y] = queen;
        board.Squares[queenPos.X, queenPos.Y] = king;
        
        int otherColorIndex = color ? 1 : 0;
        board.CastleQueenSide[otherColorIndex] = false;
        board.CastleKingSide[otherColorIndex] = false;

        return board;
    }

    private bool GetOwnColor(Board board)
    {
        bool color = false;
        foreach (Piece piece in board.Pieces)
        {
            if (piece.Id != PieceId)
                continue;
            return piece.Color;
        }
        throw new KeyNotFoundException($"Piece with Id {PieceId} does not exist on board");
    }
}
