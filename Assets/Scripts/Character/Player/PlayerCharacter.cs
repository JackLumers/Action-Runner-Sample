using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ReferenceVariables;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Character.Player
{
    public class PlayerCharacter : BaseCharacter
    {
        [SerializeField] private IntVariable _healthVariable;
        [SerializeField] protected UnityEvent _playerHealthChangedEvent;
        [SerializeField] protected UnityEvent _playerDiedEvent;

        [SerializeField]
        private List<BaseWeaponReference> _weaponReferences = new();
        
        private int _selectedWeaponIndex = 0;
        private Transform _transform;
        private readonly List<BaseWeapon> _weapons = new();

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
        }

        public override void OnReinit()
        {
            _playerHealthChangedEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            MoveSelf(Vector3.right);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _healthVariable.Value = CharacterStats.Health;
            
            _playerHealthChangedEvent?.Invoke();
        }

        protected override void OnDied()
        {
            base.OnDied();
            _playerDiedEvent?.Invoke();
        }
        
        public void FireSelectedWeapon()
        {
            var fireDirection = -(_transform.position - LookingPoint).normalized;
            
            CurrentWeapon?.TryFire(fireDirection);
        }

        public void NextWeapon()
        {
            if (_selectedWeaponIndex + 1 >= _weapons.Count)
                _selectedWeaponIndex = 0;
            else
                _selectedWeaponIndex++;
        }
    }
}