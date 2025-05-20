using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class PlayerSetup : Resource
{
    [Export] public PieceResource[] PlayerPieces;
    
    public PlayerSetup() { }

    public PlayerSetup(PieceResource[] pieces)
    {
        PlayerPieces = pieces;
    }

    public Board ConvertToBoard(PieceResource[] enemyPieces)
    {
        Piece[] pieces = new Piece[PlayerPieces.Length + enemyPieces.Length];
        Dictionary<byte, IItem[]> itemDict = [];
        byte index = 0;
        foreach (PieceResource playerPiece in PlayerPieces)
        {
            pieces[index] = playerPiece.ConvertToPiece(index, true);
            List<IItem> items = [];
            foreach (GodotItem item in playerPiece.Items)
                items.Add(item.GetItem(index));
            if (items.Count > 0)
                itemDict.Add(index, items.ToArray());
            index++;
        }
        foreach (PieceResource enemyPiece in enemyPieces)
        {
            pieces[index] = enemyPiece.ConvertToPiece(index, false);
            List<IItem> items = [];
            foreach (GodotItem item in enemyPiece.Items)
                items.Add(item.GetItem(index));
            if (items.Count > 0)
                itemDict.Add(index, items.ToArray());
            index++;
        }

        return new Board(0, pieces, [true, true], [true, true], itemDict);
    }

    public PieceResource GetPieceOnPosition(Vector2I position)
    {
        foreach (PieceResource playerPiece in PlayerPieces)
        {
            if (playerPiece.StartPosition == position)
                return playerPiece;
        }
        return null;
    }

    public Piece[] GetAsPieces()
    {
        List<Piece> pieces = [];
        byte index = 0;
        foreach (PieceResource playerPiece in PlayerPieces)
        {
            pieces.Add(playerPiece.ConvertToPiece(index, true));
            index++;
        }
        return pieces.ToArray();
    }

    public bool SetItem(Vector2I position, GodotItem item)
    {
        PieceResource piece = GetPieceOnPosition(position);
        if (piece is null)
            return false;
        piece.AddItem(item);
        return true;
    }

    public bool SetMovement(Vector2I position, GodotMovement movement)
    {
        PieceResource piece = GetPieceOnPosition(position);
        if (piece is null)
            return false;
        piece.AddMovement(movement);
        return true;
    }

    public bool AddPiece(Vector2I position, BasePiece pieceType)
    {
        if (GetPieceOnPosition(position) is not null)
            return false;
        
        PieceResource piece = PieceResource.CreateFromBasePiece(pieceType, position);
        List<PieceResource> newPieces = PlayerPieces.ToList();
        newPieces.Add(piece);
        PlayerPieces = newPieces.ToArray();

        return true;
    }
}
