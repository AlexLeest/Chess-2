using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
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
    [Export] public GodotMovement[] Movement = [];
    
    public PieceResource() { }
    
    public PieceResource(BasePiece basePiece, Vector2I startPos, GodotMovement[] movement, GodotItem[] items)
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
        Items = newItems;
    }

    public Piece ConvertToPiece(byte id, bool color)
    {
        List<IMovement> movement = [DefaultMovements.Get(PieceType)];
        foreach (GodotMovement mov in Movement)
            movement.Add(mov.GetMovement());
        SpecialPieceTypes pieceType = PieceType switch
        {
            BasePiece.KING => SpecialPieceTypes.KING,
            BasePiece.PAWN => SpecialPieceTypes.PAWN,
            _ => SpecialPieceTypes.NONE,
        };
        if (pieceType == SpecialPieceTypes.NONE)
        {
            if (StartPosition == new Vector2I(0, 0) || StartPosition == new Vector2I(0, 7))
                pieceType = SpecialPieceTypes.QUEEN_SIDE_CASTLE;
            else if (StartPosition == new Vector2I(7, 0) || StartPosition == new Vector2I(7, 7))
                pieceType = SpecialPieceTypes.KING_SIDE_CASTLE;
        }
        return new Piece(id, PieceType, color, StartPosition.ToCore(), movement.ToArray(), pieceType);
    }

    public static PieceResource CreateFromPiece(Piece piece, Board board)
    {
        return new PieceResource();
    }
}
