using UnityEngine;

namespace Enemies.BasicEnemy.Weapons.Bases
{
    public interface IWeapon
    {
        string WeaponName { get; }
        int Damage { get; }
        float TimeBetweenUses { get; }

        void Init(HandleWeapon weaponHandler);
        void Equip();
        void Use();
        void Unequip();
        void Reload();

        void TriggerDamage(Transform damageCenter);
    }
}
