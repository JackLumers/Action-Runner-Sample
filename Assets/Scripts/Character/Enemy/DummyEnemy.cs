using Events;
using UnityEngine;
using Weapons;

namespace Character.Enemy
{
    public class DummyEnemy : BaseCharacter
    {
        [SerializeField] private BaseWeaponReference _weaponReference;
        [SerializeField] private GameEvent _enemyDeathEvent;
        
        public BaseWeapon Weapon { get; private set; }
        
        private DummyEnemyAI _dummyEnemyAI;

        protected override void InheritAwake()
        {
            Weapon = _weaponReference.CreateWeaponInstance();
            Weapon.SetOwner(this);
            
            _dummyEnemyAI = new DummyEnemyAI(this);
        }

        private void FixedUpdate()
        {
            _dummyEnemyAI.OnFixedUpdate();
        }

        protected override void OnDied()
        {
            base.OnDied();
            _enemyDeathEvent?.Raise();
        }

        public override void OnCoverTaken() { }

        public override void OnCoverFreed() { }
    }
}