namespace CHESS2THESEQUELTOCHESS.scripts.core.boardevents;

public readonly struct ChangePieceTypeEvent(Piece piece, BasePiece before, BasePiece after) : IBoardEvent
{
    public uint AdjustZobristHash(uint zobristHash)
    {
        // Isn't this covered in the MovePieceEvent?
        throw new System.NotImplementedException();
    }
}
