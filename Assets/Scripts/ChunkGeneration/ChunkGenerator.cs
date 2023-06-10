using System.Collections.Generic;
using Character;
using Covers;
using ReferenceVariables;
using ToolBox.Pools;
using UnityEngine;

namespace ChunkGeneration
{
    public class ChunkGenerator : MonoBehaviour
    {
        private IntVariable _minEnemiesOnChunk;
        private IntVariable _maxEnemiesOnChunk;
        private FloatVariable _enemyHasCoverChance;
        private Chunk _chunkPrefab;
        private Cover _coverPrefab;
        private List<BaseCharacter> _possibleEnemiesPrefabs;

        private System.Random _random = new();
        
        public Chunk GenerateAndSpawn()
        {
            var chunk = _chunkPrefab.gameObject.Reuse<Chunk>();
            return chunk;
        }
    }
}