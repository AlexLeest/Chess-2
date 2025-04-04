using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotBoard : Control
{
    private Board board;
    [Export] private Color lightSquare, darkSquare;
    [Export] private int squareSize = 16;
    [Export] private GodotPiece[] pieces;
    [Export] private Texture2D[] pieceTextures;
    [Export] private Godot.Collections.Dictionary<byte, Texture2D> pieceTexturesDictionary;
    
    private Dictionary<byte, GodotPiece> pieceDictionary = new();

    public override void _Ready()
    {
        board = Board.DefaultBoard();
        pieces = new GodotPiece[board.Pieces.Length];
        for (int i = 0; i < board.Pieces.Length; i++)
        {
            Piece piece = board.Pieces[i];
            GodotPiece gdPiece = new(piece);
            pieces[i] = gdPiece;
            pieceDictionary[gdPiece.Id] = gdPiece;
            AddChild(gdPiece);
            GD.Print($"Setting piece texture for {piece.Id}");
            gdPiece.Texture = pieceTexturesDictionary[piece.Id];
            // gdPiece.Scale = new Vector2(1.5f, 1.5f);
        }
    }

    public override void _Draw()
    {
        for (int file = 0; file < 8; file++)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                Rect2 rect = new(file * squareSize, rank * squareSize, squareSize, squareSize);
                bool isLightSquare = (file + rank) % 2 == 0;
                DrawRect(rect, isLightSquare ? lightSquare : darkSquare);
            }
        }
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Input(InputEvent input)
    {
        if (input is InputEventKey key && input.IsPressed())
        {
            if (key.Keycode == Key.Q)
            {
                List<Board> moves = board.GenerateMoves();
                var randomBoard = moves[GD.RandRange(0, moves.Count - 1)];
                SetNewBoard(randomBoard);
            }
        }
    }

    public void SetNewBoard(Board newBoard)
    {
        board = newBoard;
        GodotPiece[] newPieces = new GodotPiece[newBoard.Pieces.Length];
        int newPiecesIndex = 0;
        Dictionary<byte, GodotPiece> newPieceDictionary = new();
        HashSet<GodotPiece> livePieces = new();
        
        // Where possible, match GodotPieces (based on id)
        foreach (Piece piece in board.Pieces)
        {
            GD.Print($"Piece {piece.Id}, pos {piece.Position}");
            if (pieceDictionary.TryGetValue(piece.Id, out GodotPiece gdPiece))
            {
                // Set gdPiece to new location
                newPieces[newPiecesIndex] = gdPiece;
                newPieceDictionary[piece.Id] = gdPiece;
                gdPiece.Update(piece);
                livePieces.Add(gdPiece);
                newPiecesIndex++;
            }
            // else
            // {
            //     // Spawn new GodotPiece???
            //     // This doesn't normally happen, but in theory could due to buffs/items or other whacky game mechanics
            // }
            // TODO: Find all unused pieces and delete them
        }
        foreach (GodotPiece currentPiece in pieces)
        {
            if (!livePieces.Contains(currentPiece))
            {
                currentPiece.QueueFree();
            }
        }
        pieces = newPieces;
        pieceDictionary = newPieceDictionary;
    }
}
