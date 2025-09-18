using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class PieceTextures : Resource
{
    [Export] public Texture2D SpecialMarker;
    
    [Export] private BasePieceTexture[] pieceTextures;

    private bool initialized;
    private Dictionary<(BasePiece, bool), Texture2D> textureDict;

    public Texture2D GetPieceTexture(Piece piece)
    {
        return GetPieceTexture(piece.BasePiece, piece.Color);
    }

    public Texture2D GetPieceTexture(BasePiece piece, bool color)
    {
        if (!initialized)
            InitDictionary();

        return textureDict[(piece, color)];
    }

    private void InitDictionary()
    {
        textureDict = [];
        foreach (BasePieceTexture basePiece in pieceTextures)
        {
            textureDict.Add((basePiece.BasePiece, basePiece.Color), basePiece.Texture);
        }
        initialized = true;
    }
}
