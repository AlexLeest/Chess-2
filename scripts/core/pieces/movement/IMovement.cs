using CHESS2THESEQUELTOCHESS.scripts.core.utils;
using System.Collections.Generic;

namespace CHESS2THESEQUELTOCHESS.scripts.core;

// TODO: Can these all be structs? (optimization stuff)
public interface IMovement
{
    /// <summary>
    /// Generates list of possible moves for this given the parameters
    /// </summary>
    /// <param name="id">Identifier of piece</param>
    /// <param name="from">Starting square</param>
    /// <param name="squares">Board representation</param>
    /// <param name="color">Color of the moving piece (for deciding whether a capture is possible or not)</param>
    /// <returns>List of places this movement can reach</returns>
    public List<Move> GetMovementOptions(byte id, Vector2Int from, Piece[,] squares, bool color);
    
    /// <summary>
    /// Cheaper check whether this Movement can attack a certain target square on the board
    /// </summary>
    /// <param name="from">Starting square</param>
    /// <param name="target">Target square</param>
    /// <param name="squares">Board representation</param>
    /// <param name="color">Color of moving piece</param>
    /// <returns>Boolean whether from attacks target</returns>
    public bool Attacks(Vector2Int from, Vector2Int target, Piece[,] squares, bool color);
    
    /// <summary>
    /// Cheaper check whether this movement can attack ANY of the given list of target squares
    /// </summary>
    /// <param name="from">Starting square</param>
    /// <param name="targets">List of target squares</param>
    /// <param name="squares">Board representation</param>
    /// <param name="color">Color of moving piece</param>
    /// <returns>True if yes</returns>
    public bool AttacksAny(Vector2Int from, Vector2Int[] targets, Piece[,] squares, bool color);
}