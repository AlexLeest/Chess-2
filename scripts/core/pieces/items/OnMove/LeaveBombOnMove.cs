using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnCaptured;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnTurn;
using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items.OnMove;

/// <summary>
/// Leaves a bomb behind on the spot it just moved from, which is a piece that "lives" for 1 turn and has a SelfDestruct item associated with it.
/// </summary>
/// <param name="pieceId"></param>
public class LeaveBombOnMove(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_MOVE)
{
    public override Board Execute(Board board, Move move, IBoardEvent trigger)
    {
        if (trigger is not MovePieceEvent movePieceEvent || movePieceEvent.PieceId != PieceId || board.Squares.Get(movePieceEvent.From) is not null)
            return board;

        Piece piece = board.GetPiece(PieceId);

        // BUG: Same possible problem as SpawnPawnFence, dead pieces remaining in Board.ItemsPerPiece and this bomb claiming them
        byte highestId = board.Pieces.Max(x => x.Id);

        Piece bomb = new((byte)(highestId + 1), BasePiece.BOMB, piece.Color, movePieceEvent.From, []);
        move.ApplyEvent(new SpawnPieceEvent(bomb));
        move.ApplyEvent(new AddItemEvent(bomb.Id, new SelfDestruct(bomb.Id)));
        move.ApplyEvent(new AddItemEvent(bomb.Id, new DespawnTimer(bomb.Id)));
        
        return board;
    }

    public override IItem GetNewInstance(byte pieceId)
    {
        return new LeaveBombOnMove(pieceId);
    }
}
