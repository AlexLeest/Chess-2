using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class SetupResource : Resource
{
    private BoardSetup boardSetup = BoardSetup.DefaultSetup;
    
    public List<Piece> Pieces => boardSetup.Pieces;

    public void AddPiece(Piece piece)
    {
        boardSetup.AddPiece(piece);
    }

    public bool DeletePiece(Piece piece)
    {
        return boardSetup.DeletePiece(piece);
    }
    
    public bool SetItem(byte pieceId, IItem item)
    {
        return boardSetup.SetItem(pieceId, item);
    }
}
