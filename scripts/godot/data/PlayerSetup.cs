using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class PlayerSetup : Resource
{
    [Export] public PieceResource[] PlayerPieces;

    public Board ConvertToBoard(EnemySetup enemySetup)
    {
        Piece[] pieces = new Piece[PlayerPieces.Length + enemySetup.EnemyPieces.Length];
        byte index = 0;
        foreach (PieceResource playerPiece in PlayerPieces)
        {
            pieces[index] = playerPiece.ConvertToPiece(index, true);
            index++;
        }
        foreach (PieceResource enemyPiece in enemySetup.EnemyPieces)
        {
            pieces[index] = enemyPiece.ConvertToPiece(index, false);
            index++;
        }
        
        return Board.DefaultBoard();
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
        foreach (PieceResource piece in PlayerPieces)
        {
            if (piece.StartPosition != position)
                continue;
            piece.AddItem(item);
            return true;
        }
        return false;
    }
}
