﻿using System;
using Character;
using ToolBox.Pools;
using UnityEngine;
using Weapons.Ranged.SniperRifle;

namespace Weapons.Ranged.AutoRifle
{
    public class AutoRifleWeapon : BaseRangedWeapon
    {
        private readonly System.Random _random;

        protected AutoRifleSettings AutoRifleSettings;
        
        public AutoRifleWeapon(BaseWeaponReference weaponReference, WeaponSettings weaponSettings,
            RangedWeaponSettings rangedWeaponSettings, AutoRifleSettings autoRifleSettings)
            : base(weaponReference, weaponSettings, rangedWeaponSettings)
        {
            AutoRifleSettings = autoRifleSettings;

            _random = new System.Random();
        }

        protected override void OnFire(Vector3 direction)
        {
            for (int i = 0; i < AutoRifleSettings.ShotProjectilesCount; i++)
            {
                var damageZone = WeaponSettings.DamageZonePrefab.gameObject.Reuse<DamageZone>();
                damageZone.Init(WeaponSettings.Damage, Owner.Faction);

                var ownerPosition = OwnerTransform.position;
                var shotRelativePosition = ownerPosition + direction * RangedWeaponSettings.MaxShotDistance;

                var maxAngleOffset = AutoRifleSettings.DispersionAngle / 2;
                var randomAngleOffset = _random.NextDouble() * AutoRifleSettings.DispersionAngle - maxAngleOffset;

                var calcShotRelativePositionX = (float) (Math.Cos(Mathf.Deg2Rad * randomAngleOffset) * shotRelativePosition.x -
                                                         Math.Sin(Mathf.Deg2Rad * randomAngleOffset) * shotRelativePosition.z);
                
                var calcShotRelativePositionZ = (float) (Math.Sin(Mathf.Deg2Rad * randomAngleOffset) * shotRelativePosition.x +
                                                         Math.Cos(Mathf.Deg2Rad * randomAngleOffset) * shotRelativePosition.z);

                var calcShotRelativePosition = new Vector3(calcShotRelativePositionX, shotRelativePosition.y, calcShotRelativePositionZ);

                damageZone.transform.position = ownerPosition;
                damageZone.Affected += OnDamageZoneAffected;

                damageZone.MoveTo(calcShotRelativePosition, RangedWeaponSettings.ProjectileSpeed, ReleaseDamageZone);
            }
        }

        private void OnDamageZoneAffected(DamageZone damageZone, IDamageTaker damageTaker)
        {
            ReleaseDamageZone(damageZone);
        }

        private void ReleaseDamageZone(DamageZone damageZone)
        {
            damageZone.Affected -= OnDamageZoneAffected;
            damageZone.StopMove();
            damageZone.gameObject.Release();
        }
    }
}