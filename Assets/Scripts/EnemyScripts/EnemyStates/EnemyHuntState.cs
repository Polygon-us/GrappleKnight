using System;
using UnityEngine;

public class EnemyHuntState : IState
{

    private CollisionEvents _collisionEvents;
    
    private Transform _enemyTransform;
    private Transform _playerTransform;

    private Rigidbody2D _enemyRigidbody;

    private BoxCollider2D _persecutorCollider;

    private Transform _pointA;
    private Transform _pointB;
    
    private bool _isHuntingMode;
    private bool _isOnCollision = false;
    
    private float _walkSpeed = 5f;

    private Vector2 playerDirection;

   
    private bool _isOutOfRange;
    public EnemyHuntState(BoxCollider2D persecutorCollider, Rigidbody2D enemyRigidbody, Transform enemyTransform, 
        Transform playerTransform, bool isHuntingMode,Transform pointA, Transform pointB, CollisionEvents collisionEvents)
    {
        _persecutorCollider = persecutorCollider;
        _enemyRigidbody = enemyRigidbody;
        _enemyTransform = enemyTransform;
        _playerTransform = playerTransform;
        _isHuntingMode = isHuntingMode;
        _pointA = pointA;
        _pointB = pointB;
        _collisionEvents = collisionEvents;
    }

    private bool HuntPlayer()
    {
        if (_enemyTransform.position.x > _pointA.position.x && _enemyTransform.position.x < _pointB.position.x)
        {
            if (_walkSpeed!=0 )
            {
                playerDirection = (_playerTransform.position - _enemyTransform.position).normalized;

                Vector2 moveDirection = new Vector2(playerDirection.x, 0f).normalized;

                _enemyRigidbody.velocity = moveDirection * _walkSpeed;
                return false;
            }
        }
        else if(!_isOutOfRange)
        {
            return false;
        }
        return true;
    }

    public void StartState()
    {
        _walkSpeed = 5;
        _isOnCollision = false;
        _isOutOfRange = false;
        _collisionEvents.SubscribeTriggerExit(TriggerExit);
        
    }

    private void TriggerExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isOutOfRange = true;
        }
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        if (HuntPlayer())
        {
            enemyStateEnum = EnemyStateEnum.Idle;
            return false;
        }
            
        enemyStateEnum = EnemyStateEnum.Hunt;
        return true;
    }
    
    
    private void CollisionEnter(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && !_isOnCollision )
        {
            _isOnCollision = true;
            _walkSpeed = 0;
            other.rigidbody.velocity = new Vector2(0, other.rigidbody.velocity.y);
            float directionSing = Mathf.Sign(other.transform.position.x - _enemyTransform.position.x);
            other.rigidbody.AddForce(Vector2.right*directionSing*10,ForceMode2D.Impulse);
            
        }
    }
    public Action<Collision2D> CollisionAction()
    {
        return CollisionEnter;
    }
    
    public Action<Collider2D> ColliderAction()
    {
        return null;
    }
    
}
