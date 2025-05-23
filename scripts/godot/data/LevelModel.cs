using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using CHESS2THESEQUELTOCHESS.scripts.godot.AI;
using CHESS2THESEQUELTOCHESS.scripts.godot.items;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class LevelModel : Resource
{
    [Export] public PlayerSetup EnemySetup;
    [Export] public GodotEngine[] Engines;
    [Export] private UpgradesModel upgrades;

    public PieceResource[] GetPieces(int level)
    {
        return GenerateNextLevel().PlayerPieces;
    }

    public IEngine GetEngine(int level)
    {
        level = Math.Clamp(level, 0, Engines.Length - 1);
        return Engines[level].GetEngine();
    }

    private PlayerSetup GenerateNextLevel()
    {
        // Add a random piece and item/movement

        // Pick a random piece and an unused position to spawn it on
        PlayerSetup setup = new(EnemySetup.PlayerPieces);
        
        BasePiece pieceType = upgrades.GetRandomPieceByRarity(upgrades.GetWeightedRandomItemRarity());
        Vector2I spawnPos = new(GD.RandRange(0, 7), GD.RandRange(6, 7));
        // Loop if spawnPos is already taken
        while (setup.GetPieceOnPosition(spawnPos) is not null)
        {
            spawnPos = new Vector2I(GD.RandRange(0, 7), GD.RandRange(6, 7));
        }
        setup.AddPiece(spawnPos, pieceType);
        
        // Pick a random item and apply it to a random piece
        GodotItem randomItem = upgrades.GetRandomItemByRarity(upgrades.GetWeightedRandomItemRarity());
        PieceResource randomPiece = setup.PlayerPieces[GD.RandRange(0, setup.PlayerPieces.Length - 1)];
        setup.SetItem(randomPiece.StartPosition, randomItem);

        return setup;
    }
}
