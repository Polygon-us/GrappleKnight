using System;
using Enemies.BasicEnemy.StateMachine;
using UnityEngine;

namespace Enemies.BasicEnemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class EnemyController : MonoBehaviour
    {
        private const float DETECTION_RAYS_OFFSET = 0.1f;
        private const float DETECTION_RAYS_LENGTH = 0.0001f;
        
        [Header("AI")] 
        [SerializeField] private AIBrain aiBrain;

        [Header("Body Parameters")] 
        [SerializeField][Range(0.0001f, 10f)] private float height;
        [SerializeField][Range(0.0001f, 10f)] private float width;

        private CapsuleCollider2D _collider2D;

        private void OnValidate()
        {
            _collider2D = GetComponent<CapsuleCollider2D>();
        }

        [ContextMenu("Update Body")]
        private void UpdateBody()
        {
            if(!_collider2D) return;

            _collider2D.size = new Vector2(width, height);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            float verticalOrigin = -(height / 2) + DETECTION_RAYS_OFFSET;
            float horizontalValue = (width / 2);
            
            Vector3 direction = -transform.up.normalized * DETECTION_RAYS_LENGTH;
            
            Ray groundRay = new Ray(transform.position + new Vector3(0, verticalOrigin), direction);
            
            Gizmos.color = Color.green;
            
            //Ground Ray
            Gizmos.DrawRay(groundRay);
            
            Gizmos.color = Color.blue;
            
            Ray leftRay = new Ray(transform.position + new Vector3(-horizontalValue, verticalOrigin), direction);
            
            //Left Ray
            Gizmos.DrawRay(leftRay);
            
            Ray rightRay = new Ray(transform.position + new Vector3(horizontalValue, verticalOrigin), direction);
            
            //Right Ray
            Gizmos.DrawRay(rightRay);
        }
#endif
    }
}
