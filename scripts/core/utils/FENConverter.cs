using System;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class FENConverter
{

    /// <summary>
    /// Tool for turning a FEN string into a legal board (using the default pieces)
    /// </summary>
    /// <param name="fen">FEN string</param>
    /// <returns>Resulting board</returns>
    public static Board FENToBoard(string fen)
    {
        string[] splitFen = fen.Split(' ');
        string[] ranks = splitFen[0].Split('/');
        byte id = 0;
        List<Piece> pieces = [];
        for (int index = 0; index < ranks.Length; index++)
        {
            string rankStr = ranks[index];
            int file = 0;
            int rank = 7 - index;
            foreach (char piece in rankStr)
            {
                Piece toAdd = null;
                SpecialPieceTypes castleType = file == 0 ? SpecialPieceTypes.QUEEN_SIDE_CASTLE : SpecialPieceTypes.KING_SIDE_CASTLE;
                switch (piece)
                {
                    case 'R':
                        toAdd = Piece.Rook(id, true, new Vector2Int(file, rank), castleType);
                        break;
                    case 'N':
                        toAdd = Piece.Knight(id, true, new Vector2Int(file, rank));
                        break;
                    case 'B':
                        toAdd = Piece.Bishop(id, true, new Vector2Int(file, rank));
                        break;
                    case 'Q':
                        toAdd = Piece.Queen(id, true, new Vector2Int(file, rank));
                        break;
                    case 'K':
                        toAdd = Piece.King(id, true, new Vector2Int(file, rank));
                        break;
                    case 'P':
                        toAdd = Piece.Pawn(id, true, new Vector2Int(file, rank));
                        break;
                    case 'r':
                        toAdd = Piece.Rook(id, false, new Vector2Int(file, rank), castleType);
                        break;
                    case 'n':
                        toAdd = Piece.Knight(id, false, new Vector2Int(file, rank));
                        break;
                    case 'b':
                        toAdd = Piece.Bishop(id, false, new Vector2Int(file, rank));
                        break;
                    case 'q':
                        toAdd = Piece.Queen(id, false, new Vector2Int(file, rank));
                        break;
                    case 'k':
                        toAdd = Piece.King(id, false, new Vector2Int(file, rank));
                        break;
                    case 'p':
                        toAdd = Piece.Pawn(id, false, new Vector2Int(file, rank));
                        break;
                    default:
                        file += piece - '0';
                        break;
                }
                if (toAdd != null)
                {
                    pieces.Add(toAdd);
                    file++;
                    id++;
                }
            }
        }
        int turn = int.Parse(splitFen[5]) - 1;
        if (splitFen[1] == "b")
        {
            turn++;
        }
        string castling = splitFen[2];
        bool[] castleQueenSide = [castling.Contains('Q'), castling.Contains('q')];
        bool[] castleKingSide = [castling.Contains('K'), castling.Contains('k')];

        return new Board(turn, pieces.ToArray(), castleQueenSide, castleKingSide);
    }
}
