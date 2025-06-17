using CHESS2THESEQUELTOCHESS.scripts.core.boardevents;
using CHESS2THESEQUELTOCHESS.scripts.core.pieces.items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.core.utils;

public static class ZobristCalculator
{
    private static Random rng = new();

    // TODO: Improve performance on ALL OF THESE
    private static uint[,,] basePieceHash;
    private static uint[,,] pieceTypeHash;

    private static Dictionary<(bool, Vector2Int, BasePiece ), uint> basePieceHashes = [];
    private static Dictionary<(bool, Vector2Int, SpecialPieceTypes), uint> pieceTypeHashes = [];
    private static Dictionary<(bool, Vector2Int, Type), uint> itemHashes = [];
    private static Dictionary<(bool, Vector2Int, Type), uint> movementHashes = [];

    private static uint[] castlingHashes = [RandomUint(), RandomUint(), RandomUint(), RandomUint()];
    private static uint whiteToMoveHash = RandomUint();
    
    private static bool initialized = false;

    public static void Initialize()
    {
        if (initialized)
            return;
        
        // Sets all the random uints
        basePieceHash = new uint[2, 64, (int)Enum.GetValues(typeof(BasePiece)).Cast<BasePiece>().Last() + 1];
        pieceTypeHash = new uint[2, 64, (int)Enum.GetValues(typeof(SpecialPieceTypes)).Cast<SpecialPieceTypes>().Last() + 1];

        for (int color = 0; color < 2; color++)
        {
            for (int position = 0; position < 64; position++)
            {
                foreach (BasePiece basePiece in Enum.GetValues(typeof(BasePiece)))
                {
                    basePieceHash[color, position, (int)basePiece] = RandomUint();
                }
                foreach (SpecialPieceTypes pieceType in Enum.GetValues(typeof(SpecialPieceTypes)))
                {
                    pieceTypeHash[color, position, (int)pieceType] = RandomUint();
                }
            }
        }
    }

    public static uint GetZobristHash(bool color, Vector2Int position, BasePiece piece)
    {
        return basePieceHash[color ? 0 : 1, position.ToIndex(), (int)piece];
        
        if (basePieceHashes.TryGetValue((color, position, piece), out uint hash))
            return hash;
        uint result = RandomUint();
        basePieceHashes[(color, position, piece)] = result;
        return result;
    }

    public static uint GetZobristHash(bool color, Vector2Int position, SpecialPieceTypes type)
    {
        return pieceTypeHash[color ? 0 : 1, position.ToIndex(), (int)type];
        
        if (pieceTypeHashes.TryGetValue((color, position, type), out uint hash))
            return hash;
        uint result = RandomUint();
        pieceTypeHashes[(color, position, type)] = result;
        return result;
    }

    public static uint GetZobristHash(bool color, Vector2Int position, IItem item)
    {
        if (itemHashes.TryGetValue((color, position, item.GetType()), out uint hash))
            return hash;
        uint result = RandomUint();
        itemHashes[(color, position, item.GetType())] = result;
        return result;
    }

    public static uint GetZobristHash(bool color, Vector2Int position, IMovement movement)
    {
        if (movementHashes.TryGetValue((color, position, movement.GetType()), out uint hash))
            return hash;
        uint result = RandomUint();
        movementHashes[(color, position, movement.GetType())] = result;
        return result;
    }

    public static uint GetZobristHash(bool color)
    {
        return color ? whiteToMoveHash : 0;
    }

    public static uint GetZobristHash(bool[] kingSide, bool[] queenSide)
    {
        uint result = 0;

        if (kingSide[0])
            result ^= castlingHashes[0];
        if (kingSide[1])
            result ^= castlingHashes[1];
        if (queenSide[0])
            result ^= castlingHashes[2];
        if (queenSide[1])
            result ^= castlingHashes[3];

        return result;
    }

    public static void AdjustZobristHash(Piece piece, Board board)
    {
        board.ZobristHash ^= piece.GetZobristHash();
        if (board.ItemsPerPiece.TryGetValue(piece.Id, out IItem[] items))
            foreach (IItem item in items)
                board.ZobristHash ^= item.GetZobristHash(piece.Color, piece.Position);
    }

    public static uint IncrementallyAdjustZobristHash(Board lastBoard, Board currentBoard)
    {
        // TODO: Remove this, IBoardEvent.AdjustBoard() should handle this
        
        // Instead of recalculating the whole hash from scratch, take the hash from last board and XOR out the changed elements
        uint newZobristHash = lastBoard.ZobristHash;
        Move move = currentBoard.LastMove.Value;
            
        // XOR out/in the moved piece
        newZobristHash ^= lastBoard.GetPiece(move.Moving).GetZobristHash();
        Piece movedPiece = currentBoard.GetPiece(move.Moving);
        newZobristHash ^= movedPiece.GetZobristHash();
        // Do the same for the items
        if (currentBoard.ItemsPerPiece.TryGetValue(move.Moving, out IItem[] movedItems))
        {
            foreach (IItem item in movedItems)
            {
                newZobristHash ^= item.GetZobristHash(movedPiece.Color, move.From);
                newZobristHash ^= item.GetZobristHash(movedPiece.Color, move.To);
            }
        }
            
        // If necessary, XOR out captured piece
        if (move.Captured is not null)
        {
            newZobristHash ^= move.Captured.GetZobristHash();
            if (currentBoard.ItemsPerPiece.TryGetValue(move.Captured.Id, out IItem[] capturedItems))
            {
                foreach (IItem item in capturedItems)
                {
                    newZobristHash ^= item.GetZobristHash(move.Captured.Color, move.To);
                }
            }
        }
        // XOR out/in the color-to-move
        newZobristHash ^= GetZobristHash(lastBoard.ColorToMove);
        newZobristHash ^= GetZobristHash(currentBoard.ColorToMove);
        
        // En passant decay handling
        if (lastBoard.EnPassantPawn is not null
            && currentBoard.LastMove.HasValue && currentBoard.LastMove.Value.Captured != lastBoard.EnPassantPawn)
        {
            Piece enPassantPawn = lastBoard.EnPassantPawn;
            newZobristHash ^= GetZobristHash(enPassantPawn.Color, enPassantPawn.Position, SpecialPieceTypes.EN_PASSANTABLE_PAWN);
            newZobristHash ^= GetZobristHash(enPassantPawn.Color, enPassantPawn.Position, SpecialPieceTypes.PAWN);
        }

        newZobristHash ^= GetZobristHash(lastBoard.CastleKingSide, lastBoard.CastleQueenSide);
        newZobristHash ^= GetZobristHash(currentBoard.CastleKingSide, currentBoard.CastleQueenSide);

        return newZobristHash;
    }

    public static uint RandomUint()
    {
        byte[] buffer = new byte[4];
        rng.NextBytes(buffer);
        return BitConverter.ToUInt32(buffer, 0);
    }
}
