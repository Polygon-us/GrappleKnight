using System;
using Enemies.BasicEnemy.HealthRelated.Bases;
using UnityEngine;

namespace Enemies.BasicEnemy.Weapons.Bases
{
    public enum AreaType
    {
        Circle,
        Box,
        Area
    }
    
    [CreateAssetMenu(fileName = "New MeleeWeapon", menuName = "Weapons/MeleeWeapon", order = 0)]
    public class MeleeWeapon : Weapon
    {
        [Header("Damage Zone")] 
        public AreaType areaType;

        //Common Properties
        [SerializeField] private Vector2 pointA;
        [SerializeField] private LayerMask playerLayer;
        
        //Circle
        [SerializeField] private float radius;
        
        //Box
        [SerializeField] private Vector2 size;
        [SerializeField] private float angle;
        
        //Area
        [SerializeField] private Vector2 pointB;

        public Vector2 PointA => pointA;
        public Vector2 PointB => pointB;
        public float Radius => radius;
        public Vector2 Size => size;
        public float Angle => angle;
        
        public override void TriggerDamage(Transform damageCenter)
        {
            Collider2D[] collidersHit = Array.Empty<Collider2D>();
    
            Vector2 position = damageCenter.position;
            Vector2 direction = damageCenter.right;
            float dirMultiplier = direction.x >= 0 ? 1 : -1;

            switch (areaType)
            {
                case AreaType.Circle:
                    collidersHit = Physics2D.OverlapCircleAll(position + new Vector2(dirMultiplier * pointA.x, pointA.y), radius, playerLayer);
                    break;
                case AreaType.Box:
                    Vector2 boxCenter = position + new Vector2(dirMultiplier * pointA.x, pointA.y);
                    collidersHit = Physics2D.OverlapBoxAll(boxCenter, size, angle * dirMultiplier, playerLayer);
                    break;
                case AreaType.Area:
                    Vector2 areaPointA = position + new Vector2(dirMultiplier * pointA.x, pointA.y);
                    Vector2 areaPointB = position + new Vector2(dirMultiplier * pointB.x, pointB.y);
                    collidersHit = Physics2D.OverlapAreaAll(areaPointA, areaPointB, playerLayer);
                    break;
            }

            foreach (Collider2D element in collidersHit)
            {
                if (element.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log(element.gameObject.name);
                    damageable.ReceiveDamage(Damage);
                }
            }
        }


    }
}
