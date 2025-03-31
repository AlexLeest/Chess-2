using CHESS2THESEQUELTOCHESS.scripts.core;
using Godot;

namespace CHESS2THESEQUELTOCHESS.scripts.godot;

[GlobalClass]
public partial class GodotBoard : Control
{
    private Board board;
    [Export] private string TEST;

    public override void _Ready()
    {
        board = Board.DefaultBoard();
        for (int file = 0; file < 8; file++)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                Rect2 rect = new(file * 16, rank * 16, 16, 16);
                bool isLightSquare = (file + rank) % 2 == 0;
                // BUG: Can't draw in _Ready (needs to be in update loop)
                DrawRect(rect, isLightSquare ? Colors.White : Colors.Black);
            }
        }
        
        foreach (Piece piece in board.Pieces)
        {
            
        }
    }
}
