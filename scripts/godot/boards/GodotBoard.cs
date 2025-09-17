using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotBoard : GridContainer
{
    public static int Level = -1;
    
    public Board Board;
    [Export] private PlayerSetup playerSetup;
    [Export] private LevelModel levelModel;
    [Export] private PieceTextures pieceTextures;

    [Export] private string fen;

    private IEngine engine;
    private GodotPiece[] pieces;
    private GodotSquare[,] squares;
    private Dictionary<byte, GodotSquare> pieceToSquare = new();

    private PieceTooltip tooltip;
    
    private GodotPiece selectedPiece;

    [Signal] public delegate void WinEventHandler();
    [Signal] public delegate void LossEventHandler();
    [Signal] public delegate void DrawEventHandler();

    public override void _Ready()
    {
        ZobristCalculator.Initialize();
        
        if (fen != null)
            Board = FENConverter.FENToBoard(fen);
        else
            Board = playerSetup.ConvertToBoard(levelModel.GetPieces(Level));
        squares = new GodotSquare[8, 8];
        foreach (Node node in GetChildren())
        {
            if (node is not GodotSquare square)
                continue;

            squares[square.Pos.X, square.Pos.Y] = square;
            square.SquareClicked += SquareClicked;
            square.OnMouseEntered += SquareMouseEnter;
            square.OnMouseExited += SquareMouseExit;
        }
        
        // Connect PieceTooltip
        tooltip = GetNode<PieceTooltip>("../Tooltip");
        
        RenderPieces();

        engine = levelModel.GetEngine(Level);
    }

    private void SquareMouseEnter(Vector2I coords)
    {
        Piece mousedOver = squares.Get(coords).GdPiece?.Piece;
        if (mousedOver is not null)
        {
            // Construct a PieceResource for this piece
            PieceResource toShow = PieceResource.CreateFromPiece(mousedOver, Board);

            tooltip.ShowTooltip(toShow);
        }
    }

    private void SquareMouseExit(Vector2I coords)
    {
        tooltip.HideTooltip();
    }

    private async void SquareClicked(Vector2I coords)
    {
        GD.Print($"Square clicked at {coords}");
        if (Board.Turn % 2 != 0)
        {
            GD.Print("It's black's turn");
            return;
        }
        bool colorToMove = Board.Turn % 2 == 0;
        
        Vector2Int corePos = coords.ToCore();
        if (selectedPiece != null)
        {
            Piece pieceToMove = selectedPiece.Piece;

            foreach (Move possibleMove in Board.GetMoves(pieceToMove))
            {
                if (possibleMove.From != selectedPiece?.Piece.Position || possibleMove.To != corePos)
                {
                    continue;
                }

                await SetNewBoard(possibleMove.Result);
                selectedPiece = null;
                return;
            }
            // Clicked position couldn't be moved to by this piece.
            selectedPiece = null;
        }

        GodotSquare square = squares.Get(coords);
        if (square.GdPiece == null || !square.GdPiece.Piece.Color == colorToMove)
        {
            selectedPiece = null;
            return;
        }
        selectedPiece = square.GdPiece;
        GD.Print($"Selected piece {selectedPiece.Id}");
        // TODO: Show possible moves for piece
        foreach (Move move in Board.GetMoves(selectedPiece.Piece))
        {
            // TODO: Highlight these squares
            GD.Print($"Move {move} allowed");
        }
    }

    private async Task SetNewBoard(Board newBoard)
    {
        GD.Print($"turn: {newBoard.Turn}");
        Board = newBoard;
        
        // Check for checkmate or stalemate
        if (newBoard.GetMoves().Count == 0)
        {
            string otherColor = newBoard.ColorToMove ? "Black" : "White";
            if (newBoard.IsInCheck(newBoard.ColorToMove))
            {
                // CHECKMATE
                GD.Print($"{otherColor} WINS");
                FinishLevelAndSpawnSetup(true, !newBoard.ColorToMove);
                return;
            }
            // STALEMATE
            GD.Print($"STALEMATE");
            FinishLevelAndSpawnSetup(false, false);
            return;
        }
        // GD.Print("Yeah we are now RENDER?");
        RenderPieces();
        
        if (newBoard.ColorToMove == false)
        {
            // White just played, black should respond by engine
            // TODO: Set up a "computer is thinking" visual while the engine is doing its thing
            Stopwatch stopwatch = Stopwatch.StartNew();
            Move engineResponse = await Task.Run(() => engine.GenerateNextMove(newBoard));
            // Board nextBoard = newBoard.ApplyMove(engineResponse, out List<IBoardEvent> events);
            Board nextBoard = engineResponse.Result;
            GD.Print($"Response: {nextBoard}, time: {stopwatch.Elapsed}");
            await SetNewBoard(nextBoard);
        }
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
    
    private void FinishLevelAndSpawnSetup(bool checkmate, bool color)
    {
        Node canvas = GetTree().CurrentScene;
        // TODO: Show a visual for whether this was a win, loss, or stalemate

        if (!checkmate)
        {
            // Stalemate, think of what should happen here
        }

        if (!color)
        {
            // Player lost. Reset to start of game. Actually uh how.
        }

        // Loads and spawns the preparation board
        PackedScene prepBoard = ResourceLoader.Load<PackedScene>("res://prefabs/preparation_board.tscn");
        Node board = prepBoard.Instantiate();
        canvas.AddChild(board);
        
        // kys
        QueueFree();
    }
}
