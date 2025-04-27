using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class PieceResource : Resource
{
    [Export] public Vector2I StartPosition;
    [Export] public GodotItem[] Items = [];
    [Export] public BasePiece PieceType;
    [Export] public GodotMovement[] Movement;

    public PieceResource(BasePiece basePiece, Vector2I startPos)
    {
        StartPosition = startPos;
        PieceType = basePiece;
    }

    public void AddItem(GodotItem newItem)
    {
        GodotItem[] newItems = new GodotItem[Items.Length + 1];
        int index = 0;
        foreach (GodotItem item in Items)
        {
            newItems[index] = item;
            index++;
        }
        newItems[index] = newItem;
    }

    public Piece ConvertToPiece(byte id, bool color)
    {
        List<IMovement> movement = [];
        foreach (GodotMovement mov in Movement)
        {
            movement.Add(mov.GetMovement());
        }
        return new Piece(id, PieceType, color, StartPosition.ToCore(), movement.ToArray());
    }
}
