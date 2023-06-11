using System.Collections.Generic;
using Character.Player.StateMachine;
using Events;
using JetBrains.Annotations;
using ReferenceVariables;
using UnityEngine;
using Weapons;

namespace Character.Player
{
    public class PlayerCharacter : BaseCharacter
    {
        [SerializeField] private IntVariable _healthVariable;
        [SerializeField] private GameEvent _playerHealthChangedEvent;
        [SerializeField] private GameEvent _playerDiedEvent;
        [SerializeField] private GameEvent _playerCoverTakenEvent;
        [SerializeField] private GameEvent _playerCoverFreedEvent;

        [SerializeField]
        private List<BaseWeaponReference> _weaponReferences = new();
        
        private int _selectedWeaponIndex = 0;
        private Transform _transform;
        private readonly List<BaseWeapon> _weapons = new();

        private PlayerStateMachine _playerStateMachine;

        [CanBeNull] private BaseWeapon CurrentWeapon => 
            _weapons.Count > _selectedWeaponIndex ? _weapons[_selectedWeaponIndex] : null;
        
        public Vector3 LookingPoint { get; set; }

        protected override void InheritAwake()
        {
            _transform = transform;

            foreach (var weaponReference in _weaponReferences)
            {
                var weapon = weaponReference.CreateWeaponInstance();
                weapon.SetOwner(this);
                _weapons.Add(weapon);
            }

            _healthVariable.Value = CharacterStats.Health;

            _playerStateMachine = new PlayerStateMachine(this);
            _playerStateMachine.SetState(_playerStateMachine.PlayerMovingState);
        }

        /// <inheritdoc cref="BaseCharacter.OnReinit"/>
        public override void OnReinit()
        {
            _playerHealthChangedEvent?.Raise();
        }
        
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _healthVariable.Value = CharacterStats.Health;
            
            _playerHealthChangedEvent?.Raise();
        }

        private void FixedUpdate()
        {
            _playerStateMachine.OnFixedUpdate();
        }
        
        public void OnFireInput()
        {
            _playerStateMachine.OnFireInput();
        }
        
        public void OnSwitchWeaponInput()
        {
            if (_selectedWeaponIndex + 1 >= _weapons.Count)
                _selectedWeaponIndex = 0;
            else
                _selectedWeaponIndex++;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _playerStateMachine.OnTriggerEnter(other);
        }
        
        public override void OnCoverTaken()
        {
            _playerCoverTakenEvent?.Raise();
        }

        public override void OnCoverFreed()
        {
            _playerCoverFreedEvent?.Raise();
        }

        public void OnEnemyDeath()
        {
            if (ReferenceEquals(Cover, null)) 
                return;
            
            if (_playerStateMachine.CurrentState != _playerStateMachine.PlayerBehindCoverState) 
                return;
            
            if (!Cover.Chunk.HasEnemies)
            {
                _playerStateMachine.SetState(_playerStateMachine.PlayerMovingState);
            }
        }

        public void FireSelectedWeapon()
        {
            var fireDirection = -(_transform.position - LookingPoint).normalized;
            
            CurrentWeapon?.TryFire(fireDirection);
        }
        
        protected override void OnDied()
        {
            base.OnDied();
            _playerDiedEvent?.Raise();
        }
    }
}