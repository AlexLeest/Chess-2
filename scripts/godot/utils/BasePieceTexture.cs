using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class BasePieceTexture : Resource
{
    [Export] public BasePiece BasePiece;
    [Export] public bool Color;
    [Export] public Texture2D Texture;
}
