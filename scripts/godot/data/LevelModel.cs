using CHESS2THESEQUELTOCHESS.scripts.core;
using CHESS2THESEQUELTOCHESS.scripts.core.AI;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHESS2THESEQUELTOCHESS.scripts.godot.utils;

[GlobalClass]
public partial class LevelModel : Resource
{
    // To hold all the sequential EnemySetups, which will function as levels. Possibly add random generation down the line.
    [Export] public EnemySetup[] EnemySetups;

    [Export] private UpgradesModel upgrades;

    public PieceResource[] GetPieces(int level)
    {
        while (level > EnemySetups.Length - 1)
            GenerateNextLevel();
        
        level = Math.Clamp(level, 0, EnemySetups.Length - 1);
        return EnemySetups[level].EnemyPieces.PlayerPieces;
    }

    public IEngine GetEngine(int level)
    {
        level = Math.Clamp(level, 0, EnemySetups.Length - 1);
        return EnemySetups[level].Engine.GetEngine();
    }

    private void GenerateNextLevel()
    {
        // Add a random piece and item/movement

        // Pick a random piece and an unused position to spawn it on
        BasePiece pieceType = upgrades.GetRandomPieceByRarity(upgrades.GetWeightedRandomItemRarity());
        PlayerSetup setup = new(EnemySetups[^1].EnemyPieces.PlayerPieces);
        Vector2I spawnPos = new(GD.RandRange(0, 7), GD.RandRange(6, 7));
        // Loop if spawnPos is already taken
        while (setup.GetPieceOnPosition(spawnPos) is not null)
        {
            spawnPos = new Vector2I(GD.RandRange(0, 7), GD.RandRange(6, 7));
        }
        
        setup.AddPiece(spawnPos, pieceType);

        List<EnemySetup> newSetups = EnemySetups.ToList();
        newSetups.Add(new EnemySetup(setup, EnemySetups[^1].Engine));
        EnemySetups = newSetups.ToArray();
    }
}
