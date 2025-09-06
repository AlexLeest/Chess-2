using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public class CapturePieceEvent(byte capturedPieceId, byte capturingPieceId, bool triggersEvents = true) : IBoardEvent
{
    public readonly byte CapturedPieceId = capturedPieceId;
    public readonly byte CapturingPieceId = capturingPieceId;
    
    public void AdjustBoard(Board board, Move move)
    {
        // TODO: Add ON_CAPTURE and ON_CAPTURED item triggers
        //  to note ON_CAPTURED should be triggered before actually deleting the piece off the board state (?)
        
        Piece piece = board.GetPiece(CapturedPieceId);
        
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
        if (board.Squares[piece.Position.X, piece.Position.Y].Id == CapturedPieceId)
            board.Squares[piece.Position.X, piece.Position.Y] = null;
        
        // XOR out piece hash
        board.ZobristHash ^= piece.GetZobristHash();
        
        // XOR out items
        if (board.ItemsPerPiece.TryGetValue(piece.Id, out IItem[] items))
            foreach (IItem item in items)
                board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }

    // public uint AdjustZobristHash(uint zobristHash)
    // {
    //     // XOR out piece hash
    //     zobristHash ^= piece.GetZobristHash();
    //     
    //     if (!itemDict.TryGetValue(piece.Id, out IItem[] items))
    //         return zobristHash;
    //
    //     // XOR out items
    //     foreach (IItem item in items)
    //         zobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    //
    //     return zobristHash;
    // }
}
