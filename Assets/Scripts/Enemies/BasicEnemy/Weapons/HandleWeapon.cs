using System;
using Enemies.BasicEnemy.Weapons.Bases;
using UnityEngine;

namespace Enemies.BasicEnemy.Weapons
{
    public class HandleWeapon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Weapon currentWeapon;
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private Transform weaponAttachment;
        
        public Animator TargetAnimator => targetAnimator;

        private float _timeSinceLastUse;
        private bool _canUseWeapon = true;

        private void Awake()
        {
            if (currentWeapon != null)
            {
                SetWeapon(currentWeapon);
            }
        }

        private void Update()
        {
            if (!_canUseWeapon)
            {
                _timeSinceLastUse += Time.deltaTime;

                if (_timeSinceLastUse >= currentWeapon.TimeBetweenUses)
                {
                    _canUseWeapon = true;
                }
            }
        }

        public void ChangeWeapon(Weapon newWeapon)
        {
            if(!newWeapon) return;

            if(currentWeapon)
                currentWeapon.Unequip();
            
            SetWeapon(newWeapon);
            
            currentWeapon.Equip();
            
            _canUseWeapon = true;
            _timeSinceLastUse = 0;
        }
        
        public void UseWeapon()
        {
            if (_canUseWeapon)
            {
                _canUseWeapon = false;
                _timeSinceLastUse = 0;
                
                currentWeapon.Use();
            }
        }

        public void Damage()
        {
            currentWeapon.TriggerDamage(weaponAttachment);
        }

        private void SetWeapon(Weapon newWeapon)
        {
            currentWeapon = newWeapon;
            
            currentWeapon.Init(this);
        }
    }
}
