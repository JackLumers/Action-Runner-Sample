using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ToolBox.Pools;
using UnityEngine;

namespace Runner.ChunkGeneration
{
    /// <summary>
    /// Creates and removes chunks based on player position.
    /// </summary>
    [RequireComponent(typeof(ChunkSpawner))]
    public class ChunkManager : MonoBehaviour
    {
        // TODO: Add possibility to change in runtime
        [Header("Don't change in runtime!")]
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
            {
                AddLastChunk();
            }
        }

        private void AddLastChunk()
        {
            if (ReferenceEquals(PlayerTransform, null))
            {
                throw new NullReferenceException("PlayerTransform can't be null");
            }
            
            var newChunk = _chunkSpawner.Spawn();
            newChunk.transform.SetParent(_transform, true);

            var lastChunkNode = _activeChunks.Last;
            Chunk lastChunk = null;

            if (!ReferenceEquals(lastChunkNode, null))
            {
                lastChunk = lastChunkNode.Value;
            }

            if (!ReferenceEquals(lastChunk, null))
            {
                var lastChunkEndPosition = lastChunk.End.position;
                var lastChunkPosition = lastChunk.transform.position;
                
                var newChunkPosition = new Vector3(lastChunkEndPosition.x + newChunk.BoxCollider.size.x / 2, lastChunkPosition.y,
                    lastChunkPosition.z);

                newChunk.transform.position = newChunkPosition;
            }
            else
            {
                var playerPosition = PlayerTransform.position;
                
                var newChunkPosition = new Vector3(playerPosition.x, 
                    playerPosition.y - newChunk.Up.position.y, playerPosition.z);
                
                newChunk.transform.position = newChunkPosition;
            }

            newChunk.PlayerLeftChunk += OnPlayerLeftChunk;
            
            _activeChunks.AddLast(new LinkedListNode<Chunk>(newChunk));
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
        
        /// <remarks>
        /// TODO: Support player moving direction change
        /// </remarks>
        private void OnPlayerLeftChunk(Chunk chunk)
        {
            Debug.Log("Left");
            
            chunk.PlayerLeftChunk -= OnPlayerLeftChunk;
            RemoveFirstChunk();
            AddLastChunk();
        }
    }
}