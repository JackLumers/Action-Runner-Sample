using Weapons.Ranged.Pistol;

namespace Weapons.Ranged
{
    public abstract class BaseRangedWeapon : BaseWeapon
    {
        protected RangedWeaponSettings RangedWeaponSettings;

        protected BaseRangedWeapon(BaseWeaponReference weaponReference, WeaponSettings weaponSettings,
            RangedWeaponSettings rangedWeaponSettings)
            : base(weaponReference, weaponSettings)
        {
            RangedWeaponSettings = rangedWeaponSettings;
        }
    }
}