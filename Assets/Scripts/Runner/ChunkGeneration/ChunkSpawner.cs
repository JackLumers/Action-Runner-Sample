using System.Collections.Generic;
using Character;
using Covers;
using ReferenceVariables;
using ToolBox.Pools;
using UnityEngine;

namespace Runner.ChunkGeneration
{
    public class ChunkSpawner : MonoBehaviour
    {
        private IntVariable _minEnemiesOnChunk;
        private IntVariable _maxEnemiesOnChunk;
        private FloatVariable _enemyHasCoverChance;
        [SerializeField] private Chunk _chunkPrefab;
        private Cover _coverPrefab;
        private List<BaseCharacter> _possibleEnemiesPrefabs;

        private System.Random _random = new();
        
        public Chunk Spawn()
        {
            return SpawnEmpty();
        }

        public Chunk SpawnEmpty()
        {
            var chunk = _chunkPrefab.gameObject.Reuse<Chunk>();
            chunk.Init(new List<Cover>());
            return chunk;
        }
    }
}