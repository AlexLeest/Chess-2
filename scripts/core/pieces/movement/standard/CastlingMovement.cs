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
                MovePieceEvent moveKing = new(id, new Vector2Int(6, rank));
                MovePieceEvent movePiece = new(toCastleKingSide.Id, new Vector2Int(5, rank));
                CastleEvent castle = new(color);

                castleKingSide.ApplyEvent(moveKing);
                castleKingSide.ApplyEvent(movePiece);
                castleKingSide.ApplyEvent(castle);
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
                Move castleQueenSide = new(id, from, new Vector2Int(3, rank), board);
                MovePieceEvent moveKing = new(id, new Vector2Int(3, rank));
                MovePieceEvent movePiece = new(toCastleQueenSide.Id, new Vector2Int(4, rank));
                CastleEvent castle = new(color);
                
                castleQueenSide.ApplyEvent(moveKing);
                castleQueenSide.ApplyEvent(movePiece);
                castleQueenSide.ApplyEvent(castle);
                
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
