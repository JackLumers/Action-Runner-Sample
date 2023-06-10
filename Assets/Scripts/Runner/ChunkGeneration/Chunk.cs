using System;
using System.Collections.Generic;
using Character.Player;
using Covers;
using ToolBox.Pools;
using UnityEngine;

namespace Runner.ChunkGeneration
{
    [RequireComponent(typeof(BoxCollider))]
    public class Chunk : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public Transform Start { get; private set; }
        [field: SerializeField] public Transform End { get; private set; }
        [field: SerializeField] public Transform Up { get; private set; }
        [field: SerializeField] public BoxCollider BoxCollider { get; private set; }

        private List<Cover> _covers;
        
        public event Action<Chunk> PlayerLeftChunk;

        public void Init(List<Cover> covers)
        { 
            _covers = covers;
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
            foreach (var cover in _covers)
            {
                cover.gameObject.Release();
            }

            _covers = null;
            PlayerLeftChunk = null;
        }

        private void OnDestroy()
        {
            PlayerLeftChunk = null;
        }
    }
}