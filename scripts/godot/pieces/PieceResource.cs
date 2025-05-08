using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;
using System.Linq;

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

    public void AddMovement(GodotMovement newMovement)
    {
        List<GodotMovement> newMovements = Movement.ToList();
        newMovements.Add(newMovement);
        Movement = newMovements.ToArray();
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
        PieceResource result = new();

        result.PieceType = piece.BasePiece;
        
        result.StartPosition = piece.Position.ToGodot();
        
        List<GodotMovement> gdMovement = [];
        foreach (IMovement movement in piece.Movement)
        {
            gdMovement.Add(GodotMovement.CreateFromIMovement(movement));
        }
        result.Movement = gdMovement.ToArray();

        List<GodotItem> items = [];
        if (board.ItemsPerPiece.TryGetValue(piece.Id, out IItem[] itemArray))        
            foreach (IItem item in itemArray)
                items.Add(GodotItem.CreateFromIItem(item));
        result.Items = items.ToArray();
        
        return result;
    }

    public static PieceResource CreateFromBasePiece(BasePiece piece, Vector2I startPos)
    {
        PieceResource result = new();
        
        result.StartPosition = startPos;
        result.PieceType = piece;

        return result;
    }
}
