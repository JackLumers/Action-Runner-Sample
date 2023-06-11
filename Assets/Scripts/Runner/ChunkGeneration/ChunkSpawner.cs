using System.Collections.Generic;
using Covers;
using ToolBox.Pools;
using UnityEngine;

namespace Runner.ChunkGeneration
{
    public class ChunkSpawner : MonoBehaviour
    {
        [SerializeField] private Chunk _chunkPrefab;
        [SerializeField] private Cover _enemyCoverPrefab;
        [SerializeField] private Cover _playerCoverPrefab;
        [SerializeField] [Range(1, 100)] private int _enemyCoverSpawnPossibility = 30;
        
        private System.Random _random = new();

        public Chunk Spawn()
        {
            var chunk = _chunkPrefab.gameObject.Reuse<Chunk>();
            var covers = new List<EnemyCover>();
            
            foreach (var possibleCoverPoint in chunk.EnemiesCoversPossiblePositions)
            {
                if (!(_random.Next(1, 100) >= _enemyCoverSpawnPossibility)) 
                    continue;
                
                var cover = _enemyCoverPrefab.gameObject.Reuse<EnemyCover>();

                var coverTransform = cover.transform;
                coverTransform.SetParent(chunk.transform);
                coverTransform.position = possibleCoverPoint.position;
                coverTransform.rotation = possibleCoverPoint.rotation;

                cover.SetChunk(chunk);
                covers.Add(cover);
            }

            // Spawn player cover if there is enemy covers
            if (covers.Count > 0)
            {
                var playerCover = _playerCoverPrefab.gameObject.Reuse<Cover>();
                var coverTransform = playerCover.transform;
                coverTransform.SetParent(chunk.transform);
                coverTransform.position = chunk.PlayerCoverPoint.position;
                coverTransform.rotation = chunk.PlayerCoverPoint.rotation;
                
                playerCover.SetChunk(chunk);
            }
            
            chunk.Init(covers);
            
            return chunk;
        }

        public Chunk SpawnEmpty()
        {
            var chunk = _chunkPrefab.gameObject.Reuse<Chunk>();
            chunk.Init(new List<EnemyCover>());
            return chunk;
        }
    }
}