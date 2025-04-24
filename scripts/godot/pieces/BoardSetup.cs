using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class BoardSetup : Resource
{
    public List<Piece> pieces;
    public Dictionary<byte, List<IItem>> items;

    public Board SpawnBoard()
    {
        return new Board(0, pieces.ToArray(), [true, true], [true, true], ConvertItemDict());
    }

    private Dictionary<byte, IItem[]> ConvertItemDict()
    {
        Dictionary<byte, IItem[]> result = [];
        foreach (var item in items)
        {
            result[item.Key] = item.Value.ToArray();
        }
        return result;
    }
}
