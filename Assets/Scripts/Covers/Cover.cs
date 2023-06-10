using JetBrains.Annotations;
using ToolBox.Pools;
using UnityEngine;

namespace Covers
{
    public class Cover : MonoBehaviour, IPoolable
    {
        [CanBeNull] public ICoverTaker CoverTaker;

        public void OnReuse()
        {
            
        }

        public void OnRelease()
        {
            if (CoverTaker != null) CoverTaker.Cover = null;
        }

        private void OnDestroy()
        {
            if (CoverTaker != null) CoverTaker.Cover = null;
        }
    }
}