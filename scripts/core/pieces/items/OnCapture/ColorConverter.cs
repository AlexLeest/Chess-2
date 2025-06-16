using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCapture;

public class ColorConverter(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_CAPTURE) 
{
    public override bool ConditionsMet(Board board, Move move)
    {
        Piece capturedPiece = move.Captured;
        if (capturedPiece is null)
            return false;

        Piece piece = board.GetPiece(PieceId);
        if (capturedPiece.Color == piece.Color)
            return false;
        return true;
    }

    public override Board Execute(Board board, Move move, ref List<IBoardEvent> events)
    {
        // Instead of capturing, put the piece back with your own color and don't move your own piece
        Piece piece = board.GetPiece(PieceId);
        
        Piece convertPiece = move.Captured.DeepCopy();
        Piece convertBefore = convertPiece.DeepCopy(false);
        convertPiece.Color = piece.Color;
        
        events.Add(new UpdatePieceEvent(convertBefore, convertPiece, board.ItemsPerPiece));

        Piece pieceBefore = piece.DeepCopy(false);
        piece.Position = move.From;
        board.Squares[move.From.X, move.From.Y] = piece;
        board.Squares[move.To.X, move.To.Y] = convertPiece;
        board.Pieces = board.DeepcopyPieces();
        board.Pieces[^1] = convertPiece;

        events.Add(new UpdatePieceEvent(pieceBefore, piece, board.ItemsPerPiece));

        return board;
    }
}
