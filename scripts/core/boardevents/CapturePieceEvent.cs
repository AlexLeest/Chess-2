using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class CapturePieceEvent(byte capturedPieceId, byte capturingPieceId, bool triggersEvents = true) : IBoardEvent
{
    public readonly byte CapturedPieceId = capturedPieceId;
    public readonly byte CapturingPieceId = capturingPieceId;
    
    public void AdjustBoard(Board board, Move move)
    {
        Piece piece = board.GetPiece(CapturedPieceId);

        board.ActivateItems(CapturingPieceId, ItemTriggers.BEFORE_CAPTURE, board, move, this);
        board.ActivateItems(CapturedPieceId, ItemTriggers.BEFORE_CAPTURED, board, move, this);
        
        // Removal of captured piece
        Piece[] newPieces = new Piece[board.Pieces.Length - 1];
        int index = 0;
        foreach (Piece toCopy in board.Pieces)
        {
            if (toCopy.Id == piece.Id)
                continue;
            newPieces[index] = toCopy;
            index++;
        }
        board.Pieces = newPieces;
        
        // To make sure we're only deleting the captured piece and not a piece already moved on top of it
        if (board.Squares[piece.Position.X, piece.Position.Y]?.Id == CapturedPieceId)
            board.Squares[piece.Position.X, piece.Position.Y] = null;
        
        // Activating items 
        board.ActivateItems(CapturingPieceId, ItemTriggers.AFTER_CAPTURE, board, move, this);
        board.ActivateItems(CapturedPieceId, ItemTriggers.AFTER_CAPTURED, board, move, this);

        ZobristCalculator.AdjustZobristHash(piece, board);
        // // XOR out piece hash
        // board.ZobristHash ^= piece.GetZobristHash();
        //
        // // XOR out items
        // if (board.ItemsPerPiece.TryGetValue(piece.Id, out IItem[] items))
        //     foreach (IItem item in items)
        //         board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }
}
