using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

public class CastlingMovement : IMovement
{
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Board board, bool color)
    {
        List<Move> result = [];
        bool colorToMove = board.ColorToMove;

        int colorIndex = colorToMove ? 0 : 1;
        int rank = colorToMove ? 0 : 7;
        Piece toCastleKingSide = board.Squares[7, rank];
        if (board.CastleKingSide[colorIndex] && toCastleKingSide is not null && toCastleKingSide.SpecialPieceType == SpecialPieceTypes.KING_SIDE_CASTLE)
        {
            Vector2Int[] checkPositions = [new(4, rank), new(5, rank), new(6, rank)];
            if (board.Squares[5, rank] is null && board.Squares[6, rank] is null && !board.IsInCheck(color, checkPositions))
            {
                Move castleKingSide = new(id, from, new Vector2Int(6, rank), board);

                castleKingSide.ApplyEvent(new MovePieceEvent(id, from, new Vector2Int(6, rank)));
                castleKingSide.ApplyEvent(new MovePieceEvent(toCastleKingSide.Id, toCastleKingSide.Position, new Vector2Int(5, rank)));
                castleKingSide.ApplyEvent(new CastleEvent(color));
                castleKingSide.ApplyEvent(new NextTurnEvent());
                
                result.Add(castleKingSide);
            }
        }
        Piece toCastleQueenSide = board.Squares[0, rank];
        if (board.CastleQueenSide[colorIndex] && toCastleQueenSide is not null && toCastleQueenSide.SpecialPieceType == SpecialPieceTypes.QUEEN_SIDE_CASTLE)
        {
            Vector2Int[] checkPositions = [new(4, rank), new(3, rank), new(2, rank)];
            if (board.Squares[3, rank] is null && board.Squares[2, rank] is null && board.Squares[1, rank] is null && !board.IsInCheck(color, checkPositions))
            {
                Move castleQueenSide = new(id, from, new Vector2Int(2, rank), board);
                
                castleQueenSide.ApplyEvent(new MovePieceEvent(id, from, new Vector2Int(2, rank)));
                castleQueenSide.ApplyEvent(new MovePieceEvent(toCastleQueenSide.Id, toCastleQueenSide.Position, new Vector2Int(3, rank)));
                castleQueenSide.ApplyEvent(new CastleEvent(color));
                castleQueenSide.ApplyEvent(new NextTurnEvent());
                
                result.Add(castleQueenSide);
            }
        }

        return result;
    }

    public bool Attacks(Vector2Int from, Vector2Int target, Board board, bool color)
    {
        return false;
    }

    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Board board, bool color)
    {
        return false;
    }

    public uint GetZobristHash(bool color, Vector2Int position)
    {
        return ZobristCalculator.GetZobristHash(color, position, this);
    }

    public override string ToString()
    {
        return "CASTLING";
    }
}
