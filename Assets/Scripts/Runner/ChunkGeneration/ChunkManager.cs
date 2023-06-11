using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ToolBox.Pools;
using UnityEngine;

namespace Runner.ChunkGeneration
{
    /// <summary>
    /// Creates and removes chunks around player.
    /// </summary>
    /// <remarks>
    /// Doesn't support player moving direction change, assuming player moves only forward by X coordinate.
    /// </remarks>
    [RequireComponent(typeof(ChunkSpawner))]
    public class ChunkManager : MonoBehaviour
    {
        // TODO: Add possibility to change in runtime
        [Header("Doesn't support runtime change.")]
        [Tooltip("How many chunks are already prepared before player reached them. " +
                 "Including chunk where player spawned.")]
        [SerializeField]
        [Range(1, 100)]
        private int NextChunksBuffer;

        [Tooltip("How many chunks to keep active after player passed them. " +
                 "Excluding chunk where player spawned.")]
        [SerializeField]
        [Range(1, 100)]
        private int PreviousChunksBuffer;

        [CanBeNull] public Transform PlayerTransform { get; set; }

        private ChunkSpawner _chunkSpawner;
        private Transform _transform;
        
        private LinkedList<Chunk> _activeChunks = new();

        public void Init(Transform playerTransform)
        {
            PlayerTransform = playerTransform;
            _transform = transform;
            _chunkSpawner = GetComponent<ChunkSpawner>();

            for (int i = 0; i < NextChunksBuffer; i++) 
                AddLastChunk(true);

            for (int i = 0; i < PreviousChunksBuffer; i++) 
                AddFirstChunk(true);
        }

        private void AddLastChunk(bool isEmpty)
        {
            if (ReferenceEquals(PlayerTransform, null))
                throw new NullReferenceException("PlayerTransform can't be null.");

            var newChunk = isEmpty ? _chunkSpawner.SpawnEmpty() : _chunkSpawner.Spawn();
            var chunkNode = _activeChunks.Last;
            Vector3 newChunkPosition;

            if (!ReferenceEquals(chunkNode, null))
            {
                var chunk = chunkNode.Value;
                var chunkPosition = chunk.transform.position;

                newChunkPosition = new Vector3(
                    chunkPosition.x + chunk.BoxCollider.size.x / 2 + newChunk.BoxCollider.size.x / 2, 
                    chunkPosition.y, chunkPosition.z);
            }
            else
            {
                var playerPosition = PlayerTransform.position;
                
                 newChunkPosition = new Vector3(playerPosition.x, 
                     playerPosition.y - newChunk.Up.position.y, 
                     playerPosition.z);
            }
            
            var newChunkTransform = newChunk.transform;
            newChunkTransform.SetParent(_transform, true);
            newChunkTransform.position = newChunkPosition;

            newChunk.PlayerLeftChunk += OnPlayerLeftChunk;
            
            _activeChunks.AddLast(new LinkedListNode<Chunk>(newChunk));
        }
        
        private void AddFirstChunk(bool isEmpty)
        {
            if (ReferenceEquals(PlayerTransform, null))
                throw new NullReferenceException("PlayerTransform can't be null.");

            var newChunk = isEmpty ? _chunkSpawner.SpawnEmpty() : _chunkSpawner.Spawn();
            var chunkNode = _activeChunks.First;
            Vector3 newChunkPosition;

            if (!ReferenceEquals(chunkNode, null))
            {
                var chunk = chunkNode.Value;
                var chunkPosition = chunk.transform.position;
                
                newChunkPosition = new Vector3(
                    chunkPosition.x - chunk.BoxCollider.size.x / 2 - newChunk.BoxCollider.size.x / 2, 
                    chunkPosition.y, chunkPosition.z);
            }
            else
            {
                var playerPosition = PlayerTransform.position;
                
                newChunkPosition = new Vector3(playerPosition.x, 
                    playerPosition.y - newChunk.Up.position.y, 
                    playerPosition.z);
            }
            
            var newChunkTransform = newChunk.transform;
            newChunkTransform.SetParent(_transform, true);
            newChunkTransform.position = newChunkPosition;

            newChunk.PlayerLeftChunk += OnPlayerLeftChunk;
            
            _activeChunks.AddFirst(new LinkedListNode<Chunk>(newChunk));
        }
        
        private void RemoveFirstChunk()
        {
            var chunkToRemoveNode = _activeChunks.First;
            if (!ReferenceEquals(chunkToRemoveNode, null))
            {
                var chunk = chunkToRemoveNode.Value;
                chunk.gameObject.Release();

                _activeChunks.RemoveFirst();
            }
        }
        
        private void OnPlayerLeftChunk(Chunk chunk)
        {
            chunk.PlayerLeftChunk -= OnPlayerLeftChunk;
            RemoveFirstChunk();
            AddLastChunk(false);
        }
    }
}