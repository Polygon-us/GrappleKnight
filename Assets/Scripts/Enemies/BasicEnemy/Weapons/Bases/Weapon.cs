using System.Linq;
using UnityEngine;

namespace Enemies.BasicEnemy.Weapons.Bases
{
    public abstract class Weapon : ScriptableObject, IWeapon
    {
        [Header("Weapon Parameters")] 
        [SerializeField] private string weaponName;
        [SerializeField] private int damage;
        [SerializeField] private float timeBetweenUses;

        [Header("Animator Parameters")] 
        [SerializeField] private string equipWeaponTrigger;
        [SerializeField] private string unequipWeaponTrigger;
        [SerializeField] private string useWeaponTrigger;
        [SerializeField] private string reloadWeaponTrigger;

        public string WeaponName => weaponName;
        public int Damage => damage;
        public float TimeBetweenUses => timeBetweenUses;

        protected HandleWeapon WeaponHandler { get; private set; }

        public virtual void Init(HandleWeapon weaponHandler)
        {
            WeaponHandler = weaponHandler;
        }

        public virtual void Equip()
        {
            AnimatorSetTrigger(equipWeaponTrigger);
        }

        public virtual void Use()
        {
            AnimatorSetTrigger(useWeaponTrigger);
        }
        
        public virtual void Unequip()
        {
            AnimatorSetTrigger(unequipWeaponTrigger);
        }

        public virtual void Reload()
        {
            AnimatorSetTrigger(reloadWeaponTrigger);
        }

        public abstract void TriggerDamage(Transform damageCenter);
        
        private void AnimatorSetTrigger(string triggerLabel)
        {
            if (!WeaponHandler || string.IsNullOrEmpty(triggerLabel) || string.IsNullOrWhiteSpace(triggerLabel)) return;
            
            if(!WeaponHandler.TargetAnimator) return;

            AnimatorControllerParameter param = WeaponHandler.TargetAnimator.parameters.FirstOrDefault(x =>
                x.type == AnimatorControllerParameterType.Trigger && x.name == triggerLabel);
            
            if(param != null)
                WeaponHandler.TargetAnimator.SetTrigger(param.name);
        }
    }
}
