using System;
using Globals;
using UnityEngine;

namespace Weapons.Ranged.Pistol
{
    [CreateAssetMenu(menuName = ProjectConstants.ScriptableObjectsAssetMenuName + 
                                "/" + ProjectConstants.WeaponsMenuName + 
                                "/Create new " + nameof(PistolWeaponReference))]
    public class PistolWeaponReference : BaseWeaponReference
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