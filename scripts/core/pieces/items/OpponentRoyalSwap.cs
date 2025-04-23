using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.buffs;

public class OpponentRoyalSwap(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CASTLE) 
{
    public override bool ConditionsMet(Board board, Move move)
    {
        // TODO: Check if opponent has (at least 1) queen

        return true;
    }

    public override Board Execute(Board board, Move move)
    {
        // Find out own color
        bool color = false;
        foreach (Piece piece in board.Pieces)
        {
            if (piece.Id != pieceId)
                continue;
            color = piece.Color;
            break;
        }
        
        Piece king = null;
        Piece queen = null;
        for (int index = 0; index < board.Pieces.Length; index++)
        {
            Piece piece = board.Pieces[index];
            if (piece.SpecialPieceType == SpecialPieceTypes.KING && piece.Color != color)
            {
                king = piece;
            }
            else if (piece.BasePiece == BasePiece.Queen && piece.Color != color)
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
}
