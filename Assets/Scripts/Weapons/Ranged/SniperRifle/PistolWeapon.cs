using Character;
using ToolBox.Pools;
using UnityEngine;

namespace Weapons.Ranged.SniperRifle
{
    public class PistolWeapon : BaseRangedWeapon
    {
        public PistolWeapon(BaseWeaponReference weaponReference, WeaponSettings weaponSettings,
            RangedWeaponSettings rangedWeaponSettings)
            : base(weaponReference, weaponSettings, rangedWeaponSettings)
        {
            
        }

        protected override void OnFire(Vector3 direction)
        {
            var damageZone = WeaponSettings.DamageZonePrefab.gameObject.Reuse<DamageZone>();
            damageZone.Init(WeaponSettings.Damage, Owner.Faction);

            var ownerPosition = OwnerTransform.position;
            var shotRelativePosition =
                ownerPosition + direction * RangedWeaponSettings.MaxShotDistance;
            
            damageZone.transform.position = ownerPosition;
            damageZone.Affected += OnDamageZoneAffected;

            damageZone.MoveTo(shotRelativePosition, RangedWeaponSettings.ProjectileSpeed, ReleaseDamageZone);
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