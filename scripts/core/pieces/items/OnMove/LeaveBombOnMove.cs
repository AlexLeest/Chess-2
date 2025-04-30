using CHESS2THESEQUELTOCHESS.scripts.core.buffs;

namespace CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;

/// <summary>
/// Leaves a bomb behind on the spot it just moved from, which is a piece that "lives" for 1 turn and has a SelfDestruct item associated with it.
/// </summary>
/// <param name="pieceId"></param>
public class LeaveBombOnMove(byte pieceId) : AbstractItem(pieceId, ItemTriggers.ON_MOVE)
{
    public override Board Execute(Board board, Move move)
    {
        // TODO: Think of a way to spawn a piece with a set lifespan AND different behavior on capture?
        //  - New SpecialPieceType that behaves similar to EN_PASSANTABLE_PAWN and deteriorates on DeepCopy?
        //  - Add SelfDestruct item to that piece. PROBLEM is that you don't want to add items during a game (non-mutable dict shared between boards).
        
        return board;
    }
}
