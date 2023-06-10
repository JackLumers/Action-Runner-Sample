using System;
using Globals;
using UnityEngine;

namespace Weapons.Ranged.SniperRifle
{
    [CreateAssetMenu(menuName = ProjectConstants.ScriptableObjectsAssetMenuName + 
                                "/" + ProjectConstants.WeaponsMenuName + 
                                "/Create new " + nameof(PostolWeaponReference))]
    public class PostolWeaponReference : BaseWeaponReference
    {
        [SerializeField] protected RangedWeaponSettings RangedWeaponSettings;
        
        public override BaseWeapon CreateWeaponInstance()
        {
            return new PistolWeapon(this, WeaponSettings, RangedWeaponSettings);
        }
    }
    
    [Serializable]
    public struct RangedWeaponSettings
    {
        public float ProjectileSpeed;
        public float MaxShotDistance;
    }
}