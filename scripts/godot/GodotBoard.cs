using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using CHESS2THESEQUELTOCHESS.scripts.godot.utils;
using Godot;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotBoard : GridContainer
{
    private Board board;
    [Export] private Color lightSquare, darkSquare;
    [Export] private int squareSize = 64;
    [Export] private Texture2D[] pieceTextures;
    [Export] private Godot.Collections.Dictionary<byte, Texture2D> pieceTexturesDictionary;
    
    private GodotPiece[] pieces;
    private GodotSquare[,] squares;
    private Dictionary<byte, GodotSquare> pieceToSquare = new();
    
    private GodotPiece selectedPiece;

    public override void _Ready()
    {
        board = Board.DefaultBoard();
        squares = new GodotSquare[8, 8];
        foreach (Node node in GetChildren())
        {
            if (node is not GodotSquare square)
                continue;

            squares[square.Pos.X, square.Pos.Y] = square;
            square.SquareClicked += SquareClicked;
        }
        
        pieces = new GodotPiece[board.Pieces.Length];
        for (int i = 0; i < board.Pieces.Length; i++)
        {
            Piece piece = board.Pieces[i];
            GodotPiece gdPiece = new(piece);
            pieces[i] = gdPiece;
            GodotSquare square = squares[piece.Position.X, piece.Position.Y];
            square.GdPiece = gdPiece;
            pieceToSquare[gdPiece.Id] = square;
            square.AddChild(gdPiece);
            gdPiece.Position = Vector2.Down;
            GD.Print($"Setting piece texture for {piece.Id}");
            gdPiece.Texture = pieceTexturesDictionary[piece.Id];
        }
    }

    public override void _Input(InputEvent input)
    {
        if (input is InputEventKey key && input.IsPressed())
        {
            if (key.Keycode == Key.Q)
            {
                List<Board> moves = board.GenerateMoves();
                if (moves.Count == 0)
                {
                    GD.Print("No moves found, either checkmate or draw");
                    return;
                }
                GD.Print($"Move count: {moves.Count.ToString()}");
                Board randomBoard = moves[GD.RandRange(0, moves.Count - 1)];
                SetNewBoard(randomBoard);
            }
        }
    }

    private void SquareClicked(Vector2I position)
    {
        Vector2Int corePos = position.ToCore();
        if (selectedPiece != null)
        {
            // TODO: Check if piece can move here and if so, do
            Piece pieceToMove = selectedPiece.Piece;

            foreach (IMovement movement in selectedPiece.Piece.Movement)
            {
                if (movement.GetMovementOptions(pieceToMove.Position, board.Squares, true).Contains(corePos))
                {
                    // Build Piece[], make new Board, set and go.
                    GD.Print($"Moving piece {selectedPiece.Id} to {position}");

                    Piece capturedPiece = squares[corePos.X, corePos.Y].GdPiece?.Piece;
                    Piece[] newPieces = board.DeepcopyPieces(capturedPiece == null ? [pieceToMove] : [pieceToMove, capturedPiece]);
                    newPieces[^1] = new Piece(pieceToMove.Id, pieceToMove.Color, corePos, pieceToMove.Movement);

                    Board newBoard = new Board(board.Turn + 1, newPieces);
                    SetNewBoard(newBoard);
                        
                    selectedPiece = null;
                    return;
                }
            }
            GD.Print("Unselecting");
            selectedPiece = null;
            return;
        }
        
        GodotSquare square = squares[position.X, position.Y];
        if (!square.GdPiece.Piece.Color)
        {
            // Player always plays white side so big no for this
            GD.Print("Can't control black pieces you freak");
            return;
        }
        selectedPiece = square.GdPiece;
        GD.Print($"Selected piece {selectedPiece.Id}");
        // TODO: Show possible moves for piece
        foreach (IMovement movement in selectedPiece.Piece.Movement)
        {
            foreach (Vector2Int move in movement.GetMovementOptions(corePos, board.Squares, true))
            {
                // TODO: Accentuate these
                GD.Print($"Move to {move} allowed");
            }
        }
    }

    public void SetNewBoard(Board newBoard)
    {
        board = newBoard;
        GodotPiece[] newPieces = new GodotPiece[newBoard.Pieces.Length];
        int newPiecesIndex = 0;
        Dictionary<byte, GodotSquare> newPieceToSquare = new();
        HashSet<GodotPiece> livePieces = new();
        
        // Where possible, match GodotPieces (based on id)
        foreach (Piece piece in board.Pieces)
        {
            if (pieceToSquare.TryGetValue(piece.Id, out GodotSquare gdSquare))
            {
                GodotPiece gdPiece = gdSquare.GdPiece;
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
    }
}
