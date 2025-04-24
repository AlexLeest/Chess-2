using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class PieceTextures : Resource
{
    [Export] private BasePieceTexture[] pieceTextures;

    private bool initialized;
    private Dictionary<(BasePiece, bool), Texture2D> textureDict;

    public Texture2D GetPieceTexture(BasePiece piece, bool color)
    {
        if (!initialized)
            InitDictionary();

        return textureDict[(piece, color)];
    }

    public Texture2D GetPieceTexture(Piece piece)
    {
        if (!initialized)
            InitDictionary();

        return textureDict[(piece.BasePiece, piece.Color)];
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
