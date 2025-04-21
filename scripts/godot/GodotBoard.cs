using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotBoard : GridContainer
{
    public Board Board;
    [Export] private Color lightSquare, darkSquare;
    [Export] private int squareSize = 64;
    [Export] private Godot.Collections.Dictionary<byte, Texture2D> pieceTexturesDictionary;
    [Export] private BasePieceTexture[] pieceTextures;

    [Export] private string fen;

    private IEngine engine;
    private GodotPiece[] pieces;
    private GodotSquare[,] squares;
    private Dictionary<byte, GodotSquare> pieceToSquare = new();
    
    private GodotPiece selectedPiece;

    public override void _Ready()
    {
        if (fen != null)
            Board = FENConverter.FENToBoard(fen);
        else
            Board = Board.DefaultBoard();
        squares = new GodotSquare[8, 8];
        foreach (Node node in GetChildren())
        {
            if (node is not GodotSquare square)
                continue;

            squares[square.Pos.X, square.Pos.Y] = square;
            square.SquareClicked += SquareClicked;
        }
        
        pieces = new GodotPiece[Board.Pieces.Length];
        for (int i = 0; i < Board.Pieces.Length; i++)
        {
            Piece piece = Board.Pieces[i];
            GodotPiece gdPiece = new(piece);
            pieces[i] = gdPiece;
            GodotSquare square = squares[piece.Position.X, piece.Position.Y];
            square.GdPiece = gdPiece;
            pieceToSquare[gdPiece.Id] = square;
            square.AddChild(gdPiece);
            gdPiece.Position = Vector2.Down;
            // GD.Print($"Setting piece texture for {piece.Id}");
            
            // Determine piece type (only standard pieces for now)
            foreach (BasePieceTexture texture in pieceTextures)
            {
                if (!(piece.BasePiece == texture.BasePiece && piece.Color == texture.Color))
                    continue;
                gdPiece.Texture = texture.Texture;
                break;
            }
            // gdPiece.Texture = pieceTexturesDictionary[piece.Id];
        }

        engine = new FullRandom();
    }

    private void SquareClicked(Vector2I position)
    {
        // if (board.Turn % 2 != 0)
        // {
        //     GD.Print("It's black's turn");
        //     return;
        // }
        bool colorToMove = Board.Turn % 2 == 0;
        
        Vector2Int corePos = position.ToCore();
        if (selectedPiece != null)
        {
            Piece pieceToMove = selectedPiece.Piece;

            foreach (Board possibleBoard in Board.GenerateMoves(pieceToMove))
            {
                if (possibleBoard.Squares[corePos.X, corePos.Y]?.Id != pieceToMove.Id)
                    continue;
                
                // BUG: Self-destruct actually prevents this from happening because oops, pieceToMove is in fact fuckn dead.
                SetNewBoard(possibleBoard);
                selectedPiece = null;
                return;
            }
            // Clicked position couldn't be moved to by this piece.
            selectedPiece = null;
        }
        
        GodotSquare square = squares[position.X, position.Y];
        if (square.GdPiece == null || !square.GdPiece.Piece.Color == colorToMove)
        {
            selectedPiece = null;
            return;
        }
        selectedPiece = square.GdPiece;
        GD.Print($"Selected piece {selectedPiece.Id}");
        // TODO: Show possible moves for piece
        foreach (Vector2Int move in selectedPiece.Piece.GetMovementOptions(Board.Squares))
        {
            // TODO: Accentuate these
            GD.Print($"Move to {move} allowed");
        }
    }

    private void SetNewBoard(Board newBoard)
    {
        Board = newBoard;
        GodotPiece[] newPieces = new GodotPiece[newBoard.Pieces.Length];
        int newPiecesIndex = 0;
        Dictionary<byte, GodotSquare> newPieceToSquare = new();
        HashSet<GodotPiece> livePieces = new();
        
        // Where possible, match GodotPieces (based on id)
        foreach (Piece piece in Board.Pieces)
        {
            if (pieceToSquare.TryGetValue(piece.Id, out GodotSquare gdSquare))
            {
                GodotPiece gdPiece = gdSquare.GdPiece;
                gdPiece.Piece = piece;
                // Set gdPiece to new location
                newPieces[newPiecesIndex] = gdPiece;
                GodotSquare newSquare = squares[piece.Position.X, piece.Position.Y];
                
                gdSquare.GdPiece = null;
                newSquare.GdPiece = gdPiece;
                gdPiece.Reparent(newSquare, false);

                newPieceToSquare[piece.Id] = newSquare;
                livePieces.Add(gdPiece);
                newPiecesIndex++;
            }
            // else
            // {
            //     // Spawn new GodotPiece???
            //     // This doesn't normally happen, but in theory could due to buffs/items or other whacky game mechanics
            // }
        }
        foreach (GodotPiece currentPiece in pieces)
        {
            if (!livePieces.Contains(currentPiece))
            {
                currentPiece.QueueFree();
            }
        }
        pieces = newPieces;
        pieceToSquare = newPieceToSquare;

        // if (board.Turn % 2 != 0)
        // {
        //     // White just played, black should respond by engine
        //     // TODO: Skip rendering out the whole damn board if you're going another step down anyway
        //     var engineResponse = engine.GenerateNextMove(board);
        //     SetNewBoard(engineResponse);
        // }
    }
}
