using JetBrains.Annotations;
using Runner.ChunkGeneration;
using ToolBox.Pools;
using UnityEngine;

namespace Covers
{
    public class Cover : MonoBehaviour, IPoolable, IChunkPart
    {
        [field: SerializeField] public BoxCollider TriggerCollider { get; private set; }
        [field: SerializeField] public BoxCollider ModelCollider { get; private set; }
        [field: SerializeField] public Transform LeftExposedPoint { get; private set; }
        [field: SerializeField] public Transform RightExposedPoint { get; private set; }
        [field: SerializeField] public Transform UnexposedPoint { get; private set; }

        [CanBeNull] public ICoverTaker CoverTaker { get; private set; }
        
        public Chunk Chunk { get; protected set; }

        public bool IsOccupied => CoverTaker != null;

        public void SetChunk(Chunk chunk)
        {
            Chunk = chunk;
        }
        
        public void Occupy([NotNull] ICoverTaker coverTaker)
        {
            if (IsOccupied)
            {
                Debug.LogError("Cover is already occupied.");
                return;
            }
            
            CoverTaker = coverTaker;
            coverTaker.Cover = this;
            CoverTaker.OnCoverTaken();
        }

        public void Free()
        {
            if (CoverTaker != null && CoverTaker.Cover == this)
            {
                CoverTaker.Cover = null;
                CoverTaker.OnCoverFreed();
            }

            CoverTaker = null;
        }

        public void OnReuse()
        {
            OnReuseInherit();
        }

        public void OnRelease()
        {
            OnReleaseInherit();
            Free();
        }

        public virtual void OnReuseInherit(){}
        public virtual void OnReleaseInherit(){}

        private void OnDestroy()
        {
            Free();
        }
    }
}