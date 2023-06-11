using System;
using System.Collections.Generic;
using System.Linq;
using Character.Player;
using Covers;
using ToolBox.Pools;
using UnityEngine;

namespace Runner.ChunkGeneration
{
    [RequireComponent(typeof(BoxCollider))]
    public class Chunk : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public Transform Up { get; private set; }
        [field: SerializeField] public BoxCollider BoxCollider { get; private set; }
        [field: SerializeField] public Transform PlayerCoverPoint { get; private set; }
        
        [SerializeField] private List<Transform> _enemiesCoversPossiblePositions = new();

        private List<EnemyCover> _enemyCovers;

        public IReadOnlyList<Transform> EnemiesCoversPossiblePositions => _enemiesCoversPossiblePositions.AsReadOnly();
        public bool HasEnemies => _enemyCovers.Any(cover => cover.IsOccupied);

        public event Action<Chunk> PlayerLeftChunk;

        public void Init(List<EnemyCover> covers)
        { 
            _enemyCovers = covers;
        }

        public void OnReuse() { }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerModel>(out _))
            {
                PlayerLeftChunk?.Invoke(this);
            }
        }
        
        public void OnRelease()
        {
            foreach (var cover in _enemyCovers)
            {
                cover.gameObject.Release();
            }

            _enemyCovers = null;
            PlayerLeftChunk = null;
        }

        private void OnDestroy()
        {
            PlayerLeftChunk = null;
        }
    }
}