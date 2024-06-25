using Enemies.BasicEnemy.HealthRelated;
using Enemies.BasicEnemy.StateMachine;
using Enemies.BasicEnemy.Weapons;
using UnityEngine;

namespace Enemies.BasicEnemy
{
    public enum EDirection
    {
        Right,
        Left
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(HandleWeapon))]
    public class EnemyController : MonoBehaviour
    {
        private const float DETECTION_RAYS_OFFSET = 0.5f;
        private const float DETECTION_RAYS_LENGTH = 1f;
        
        [Header("AI")] 
        [SerializeField] private AIBrain aiBrain;

        [Header("Body Parameters")] 
        [SerializeField][Range(0.0001f, 10f)] private float height;
        [SerializeField][Range(0.0001f, 10f)] private float width;

        [Header("Physics")] 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] [Range(0, 5)] private byte extraGroundDetectors;
        [Space]
        [SerializeField] private LayerMask blockLayer;

        private Health _health;
        private HandleWeapon _handleWeapon;
        
#if UNITY_EDITOR
        private CapsuleCollider2D _collider2D;  
#endif
        
        public bool Grounded { get; private set; }
        public bool CanWalk { get; private set; }
        public bool Blocked { get; private set; }

        public EDirection Direction
        {
            get => transform.rotation.y == 0 ? EDirection.Right : EDirection.Left;
            private set
            {
                if (value == EDirection.Right)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }

        public HandleWeapon HandleWeapon => _handleWeapon;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _collider2D = GetComponent<CapsuleCollider2D>();
            
            UpdateBody();
        }

        [ContextMenu("Update Body")]
        private void UpdateBody()
        {
            if(!_collider2D) return;

            if (width > height) height = width;

            _collider2D.size = new Vector2(width, height);
        }  
#endif
        private void Awake()
        {
            _health = GetComponent<Health>();
            _handleWeapon = GetComponent<HandleWeapon>();
            
            if (aiBrain) aiBrain.InitMachine(this);
            
            AddListeners();
        }

        private void FixedUpdate()
        {
            Grounded = IsGrounded();

            CanWalk = FloorInFront();

            Blocked = IsBlocked();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        #region Behaviors

        [ContextMenu("Change Direction")]
        public void ChangeDirection()
        {
            Direction = Direction == EDirection.Right ? EDirection.Left : EDirection.Right;
        }
        
        public void ChangeDirection(EDirection newDirection)
        {
            Direction = newDirection;
        }

        #endregion
        
        #region Utility

        private bool IsGrounded()
        {
            Vector3 centerOrigin = transform.position + (Vector3.down * ((height/2) - DETECTION_RAYS_OFFSET));
            float rayLength = Mathf.Min((DETECTION_RAYS_LENGTH * height)/2, DETECTION_RAYS_LENGTH);

            if (Physics2D.Raycast(centerOrigin, Vector3.down, rayLength, groundLayer)) return true;
            
            if (extraGroundDetectors > 0)
            {
                float horizontalSpacing = (width / 2) / (extraGroundDetectors + 1);

                for (int i = 1; i <= extraGroundDetectors; i++)
                {
                    if (Physics2D.Raycast(centerOrigin + (Vector3.left * (horizontalSpacing * i)), 
                            Vector3.down,
                            rayLength, 
                            groundLayer) || 
                        Physics2D.Raycast(centerOrigin + (Vector3.right * (horizontalSpacing * i)), 
                            Vector3.down, 
                            rayLength, 
                            groundLayer))
                        return true;
                }
            }

            return false;
        }
        
        private bool FloorInFront()
        {
            Vector3 centerOrigin = transform.position + (Vector3.down * ((height/2) - DETECTION_RAYS_OFFSET));

            float horizontalValue = (width / 2);
            
            centerOrigin += ((Direction == EDirection.Right ? Vector3.right : Vector3.left) * horizontalValue);
             
            float rayLength = Mathf.Min((DETECTION_RAYS_LENGTH * height)/2, DETECTION_RAYS_LENGTH);

            return Physics2D.Raycast(centerOrigin, Vector3.down, rayLength, groundLayer);
        }
        
        private bool IsBlocked()
        {
            Vector3 sideOrigin = transform.position + (Direction == EDirection.Right ? Vector3.right : Vector3.left) * (width / 2);

            return Physics2D.Raycast(
                sideOrigin, 
                Direction == EDirection.Right ? Vector3.right : Vector3.left, 
                DETECTION_RAYS_LENGTH / 2, 
                blockLayer);
        }

        private void AddListeners()
        {
            _health.Died.AddListener(aiBrain.StopMachine);
        }

        private void RemoveListeners()
        {
            _health.Died.RemoveListener(aiBrain.StopMachine);
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 centerOrigin = transform.position + (Vector3.down * ((height/2) - DETECTION_RAYS_OFFSET));
            Vector3 rayDirection = Vector3.down * Mathf.Min((DETECTION_RAYS_LENGTH * height)/2, DETECTION_RAYS_LENGTH);
            
            float horizontalValue = (width / 2);
            
            Gizmos.color = Color.green;
            
            //Ground Ray
            Gizmos.DrawRay(centerOrigin, rayDirection);

            if (extraGroundDetectors > 0)
            {
                // Calculate the spacing between the rays
                float horizontalSpacing = (width / 2) / (extraGroundDetectors + 1);

                for (int i = 1; i <= extraGroundDetectors; i++)
                {
                    // Left side rays
                    Gizmos.DrawRay(centerOrigin + (Vector3.left * (horizontalSpacing * i)), rayDirection);
                    // Right side rays
                    Gizmos.DrawRay(centerOrigin + (Vector3.right * (horizontalSpacing * i)), rayDirection);
                }
            }
            
            Gizmos.color = Color.blue;
            
            //Left Ground Ray
            Gizmos.DrawRay(centerOrigin + (Vector3.left * horizontalValue), rayDirection);
            
            //Right Ground Ray
            Gizmos.DrawRay(centerOrigin + (Vector3.right * horizontalValue), rayDirection);
            
            Gizmos.color = Color.red;
            
            Vector3 sideROrigin = transform.position + (Vector3.right * (width/2));
            Vector3 sideLOrigin = transform.position + (Vector3.left * (width/2));
            
            //Left Obstacle Ray
            Gizmos.DrawRay(sideLOrigin, Vector3.left * DETECTION_RAYS_LENGTH/2);
            
            //Right Obstacle Ray
            Gizmos.DrawRay(sideROrigin, Vector3.right * DETECTION_RAYS_LENGTH/2);
        }
#endif
    }
}