using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public class BoardSetup(List<Piece> pieces, Dictionary<byte, List<IItem>> items)
{
    public List<Piece> Pieces = pieces;
    public Dictionary<byte, List<IItem>> Items = items;

    /// <summary>
    /// Default starting setup with the kingside pieces minus rook/pawn, no items.
    /// </summary>
    public static BoardSetup DefaultSetup
    {
        get
        {
            Piece[] pieces =
            [
                Piece.King(0, true, new Vector2Int(4, 0)),
                Piece.Bishop(1, true, new Vector2Int(5, 0)),
                Piece.Knight(2, true, new Vector2Int(6, 0)),

                Piece.Pawn(3, true, new Vector2Int(4, 1)),
                Piece.Pawn(4, true, new Vector2Int(5, 1)),
                Piece.Pawn(5, true, new Vector2Int(6, 1)),
            ];
            return new BoardSetup(pieces.ToList(), []);
        }
    }

    public Board SpawnBoard()
    {
        return new Board(0, Pieces.ToArray(), [true, true], [true, true], ConvertItemDict());
    }

    private Dictionary<byte, IItem[]> ConvertItemDict()
    {
        Dictionary<byte, IItem[]> result = [];
        foreach (KeyValuePair<byte, List<IItem>> item in Items)
        {
            result[item.Key] = item.Value.ToArray();
        }
        return result;
    }

    public void AddPiece(Piece piece)
    {
        Pieces.Add(piece);
    }

    public bool DeletePiece(Piece piece)
    {
        // Removes the piece itself and all items associated
        Items.Remove(piece.Id);
        return Pieces.Remove(piece);
    }

    public bool SetItem(Piece piece, IItem item)
    {
        return SetItem(piece.Id, item);
    }
    
    public bool SetItem(byte pieceId, IItem item)
    {
        if (Pieces.All(p => p.Id != pieceId))
            return false;
        
        if (Items.TryGetValue(pieceId, out List<IItem> currentItems))
            currentItems.Add(item);
        else
            Items.Add(pieceId, [item]);

        return true;
    }
}
