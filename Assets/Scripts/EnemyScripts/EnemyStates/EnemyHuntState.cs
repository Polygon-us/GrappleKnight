using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyHuntState : IState
{
    private CollisionEvents _collisionEvents;
    
    private Transform _enemyTransform;
    private Transform _playerTransform;

    private SpriteRenderer _damageColor;

    private Rigidbody2D _enemyRigidbody;

    private BoxCollider2D _persecutorCollider;

    private Transform _pointA;
    private Transform _pointB;
    
    private bool _isHuntingMode;
    private bool _isOnCollision = false;
    
    [SerializeField] private float _knockback = 10f;
    private float _walkSpeed = 5f;
    private float _walkSpeedHunting = 7f;

    private Vector3 _diagonalForce;
    private Vector2 playerDirection;

    private int _percentDamage;
    
    private bool _isOutOfRange;
    public EnemyHuntState(BoxCollider2D persecutorCollider, Rigidbody2D enemyRigidbody, Transform enemyTransform, 
        Transform playerTransform, bool isHuntingMode,Transform pointA, Transform pointB, CollisionEvents collisionEvents,
        int percentDamage)
    {
        _persecutorCollider = persecutorCollider;
        _enemyRigidbody = enemyRigidbody;
        _enemyTransform = enemyTransform;
        _playerTransform = playerTransform;
        _isHuntingMode = isHuntingMode;
        _pointA = pointA;
        _pointB = pointB;
        _collisionEvents = collisionEvents;
        _percentDamage = percentDamage;
    }

    private bool HuntPlayer()
    {
        if (_enemyTransform.position.x > _pointA.position.x && _enemyTransform.position.x < _pointB.position.x)
        {
            if (_walkSpeed!=0 )
            {
                playerDirection = (_playerTransform.position - _enemyTransform.position).normalized;

                Vector2 moveDirection = new Vector2(playerDirection.x, 0f).normalized;

                _enemyRigidbody.velocity = moveDirection * _walkSpeedHunting;
                return false;
            }
           else if(!_isOutOfRange)
            {
                return false;
            }
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
            Vector3 _diagonalForce = new Vector3(1f * directionSing, 1f, 0f).normalized * _knockback;
            other.rigidbody.AddForce(_diagonalForce,ForceMode2D.Impulse);
            other.transform.GetComponent<ILife>().ReduceLife(_percentDamage);
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
