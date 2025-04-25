using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.buffs;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class PreparationBoard : GridContainer
{
    // Purpose of this thing:
    // Show ranks 1 and 2 of a board (white starting row) with the player's current setup
    // Allow player to shuffle pieces around at will
    [Export] private PieceTextures pieceTextures;
    [Export] private SetupResource boardSetup;
    
    // private Piece[] pieces;
    private GodotSquare[,] squares;

    private GodotSquare selectedSquare;

    public override void _Ready()
    {
        // TODO: Do I try and grab a savegame from somewhere? Save this to a resource?
        
        squares = new GodotSquare[8, 2];
        foreach (Node node in GetChildren())
        {
            if (node is not GodotSquare square)
                continue;

            squares[square.Pos.X, square.Pos.Y] = square;
            square.SquareClicked += SquareClicked;
        }
        
        RenderPieces();
    }

    public bool SetItem(IItem item, Vector2I coords)
    {
        Piece subject = squares[coords.X, coords.Y].GdPiece?.Piece;
        if (subject is not null)
            return boardSetup.SetItem(subject.Id, item);
        return false;
    }

    public void DeletePiece(Piece piece)
    {
        boardSetup.DeletePiece(piece);
    }

    private void SquareClicked(Vector2I position)
    {
        // De-highlight square if one was selected before this
        
        GodotSquare clickedSquare = squares[position.X, position.Y];
        
        if (selectedSquare is not null)
        {
            // Swap selectedSquare and onPos
            Vector2I selectedPos = selectedSquare.Pos;
            Vector2I clickedPos = clickedSquare.Pos;
            
            Piece selectedPiece = selectedSquare.GdPiece.Piece;
            selectedPiece.Position = clickedPos.ToCore();
            Piece clickedPiece = clickedSquare.GdPiece.Piece;
            clickedPiece.Position = selectedPos.ToCore();
            
            RenderPieces();
            return;
        }
        if (clickedSquare.GdPiece is not null)
        {
            selectedSquare = clickedSquare;
            // Highlight the square

            return;
        }

        selectedSquare = null;
        // No need to select empty squares
    }

    private void RenderPieces()
    {
        foreach (GodotSquare square in squares)
            square.Clear();

        foreach (Piece piece in boardSetup.Pieces)
        {
            GodotSquare square = squares[piece.Position.X, piece.Position.Y];
            GodotPiece gdPiece = new(piece);
            square.GdPiece = gdPiece;
            square.AddChild(gdPiece);
            gdPiece.Texture = pieceTextures.GetPieceTexture(piece);
        }
    }
}
