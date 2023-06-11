using Character;
using UnityEngine;

namespace Covers
{
    // TODO: Support enemies without covers.
    // For now all enemies covers are occupied by an enemy and only covers can be spawned.
    public class EnemyCover : Cover
    {
        [SerializeField] private BaseCharacter _enemy;

        private void Awake()
        {
            Occupy(_enemy);
        }

        public override void OnReuseInherit()
        {
            base.OnReuseInherit();
            
            Occupy(_enemy);
            
            _enemy.Reinit();
            _enemy.gameObject.SetActive(true);
        }
    }
}