﻿using System;
using Globals;
using UnityEngine;
using Weapons.Ranged.SniperRifle;

namespace Weapons.Ranged.AutoRifle
{
    [CreateAssetMenu(menuName = ProjectConstants.ScriptableObjectsAssetMenuName + 
                                "/" + ProjectConstants.WeaponsMenuName + 
                                "/Create new " + nameof(AutoRifleReference))]
    public class AutoRifleReference : BaseWeaponReference
    {
        [SerializeField] protected RangedWeaponSettings RangedWeaponSettings;
        [SerializeField] protected AutoRifleSettings AutoRifleSettings;

        public override BaseWeapon CreateWeaponInstance()
        {
            return new AutoRifleWeapon(this, WeaponSettings, RangedWeaponSettings, AutoRifleSettings);
        }
    }
    
    [Serializable]
    public struct AutoRifleSettings
    {
        public float DispersionAngle;
        public float ShotProjectilesCount;
    }
}