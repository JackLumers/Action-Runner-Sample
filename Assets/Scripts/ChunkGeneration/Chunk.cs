using System.Collections.Generic;
using Covers;
using ToolBox.Pools;
using UnityEngine;

namespace ChunkGeneration
{
    public class Chunk : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public Transform Start { get; private set; }
        [field: SerializeField] public Transform End { get; private set; }

        private List<Cover> _covers;

        public void Init(List<Cover> covers)
        {
            
        }

        public void OnReuse()
        {
        }

        public void OnRelease()
        {
            foreach (var cover in _covers)
            {
                cover.gameObject.Release();
            }
        }
    }
}