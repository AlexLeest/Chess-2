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
    [Export] private PlayerSetup boardPlayerSetup;
    [Export] private PieceTextures pieceTextures;

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
            // Board = boardPlayerSetup.ConvertToBoard();
            Board = Board.DefaultBoard();
        squares = new GodotSquare[8, 8];
        foreach (Node node in GetChildren())
        {
            if (node is not GodotSquare square)
                continue;

            squares[square.Pos.X, square.Pos.Y] = square;
            square.SquareClicked += SquareClicked;
        }
        
        RenderPieces();

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
                if (possibleBoard.LastMove?.From != selectedPiece?.Piece.Position || possibleBoard.LastMove?.To != corePos)
                    continue;
                
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
        foreach (Move move in selectedPiece.Piece.GetMovementOptions(Board.Squares))
        {
            // TODO: Accentuate these
            GD.Print($"Move {move} allowed");
        }
    }

    private void SetNewBoard(Board newBoard)
    {
        Board = newBoard;
        RenderPieces();

        // if (board.Turn % 2 != 0)
        // {
        //     // White just played, black should respond by engine
        //     // TODO: Skip rendering out the whole damn board if you're going another step down anyway
        //     var engineResponse = engine.GenerateNextMove(board);
        //     SetNewBoard(engineResponse);
        // }
    }

    private void RenderPieces()
    {
        foreach (GodotSquare square in squares)
        {
            square.Clear();
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
            
            gdPiece.Texture = pieceTextures.GetPieceTexture(piece);
        }
    }
}
