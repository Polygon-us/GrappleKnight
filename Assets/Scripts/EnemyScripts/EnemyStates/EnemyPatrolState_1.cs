using UnityEngine;
using System.Collections;
using System;
using Unity.Mathematics;
public class EnemyPatrolState : IState
{
    [Header("EnemyPatrolState")]
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float slowdownDistance = 1f; 
     private float minDistance = 0.3f; 

    [Space]
    [Header("Movement")]
    [SerializeField] private float walkHorizontalSpeed = 4f;
    private bool isWaiting = false;

    [Space]
    [Header("References")]
    private Transform pointA;
    private Transform pointB;
    private Rigidbody2D _enemyRigidbody;
    private Transform _transform;
    private Transform targetPoint;

    private CollisionEvents _collisionEvents;

    private bool _isPlayerDetected;
   
    public EnemyPatrolState(Transform pointA, Transform pointB, Rigidbody2D enemyRigidbody, Transform transform,CollisionEvents collisionEvents)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        _enemyRigidbody = enemyRigidbody;
        _transform = transform;
        SetTargetPoint(pointB);
        _collisionEvents = collisionEvents;
    }

    public void StartState()
    {
        _isPlayerDetected = false;
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {

        if (_isPlayerDetected)
        {
            if (_transform.position.x > pointA.position.x && _transform.position.x < pointB.position.x)
            {
                _enemyRigidbody.velocity = Vector3.zero;
                enemyStateEnum = EnemyStateEnum.Hunt;
                return false;
            }
        }
        if (!isWaiting)
        {
            SetTargetPoint((targetPoint == pointA) ? pointB : pointA);
            isWaiting = true;
           
        }
        if (MoveToTargetPoint())
        {
            isWaiting = false;
            enemyStateEnum = EnemyStateEnum.Idle;
            return false;
          
        }
        enemyStateEnum = EnemyStateEnum.Idle;
        return true;
    }
    
    public Action<Collision2D> CollisionAction()
    {
        return null;
    }

    public Action<Collider2D> ColliderAction()
    {
        return ColliderEnter;
    }
    
    private void ColliderEnter(Collider2D other)
    {
        if (other.CompareTag("Player") && (_transform.position.x > pointA.position.x && _transform.position.x < pointB.position.x))
        {
            if (other.transform.position.x < _transform.position.x) 
            {
                _transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                _transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if ((_transform.position.x > pointA.position.x && _transform.position.x < pointB.position.x))
            {
                _isPlayerDetected = true;

            }
        }
    } 
 
    void SetTargetPoint(Transform newTargetPoint)
    {
        targetPoint = newTargetPoint;
        Debug.Log(targetPoint);
        if (targetPoint == pointA)
        {
            _transform.rotation = Quaternion.Euler(0,180,0);
        }
        else if (targetPoint == pointB)
        {
            _transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    bool MoveToTargetPoint()
    {
        if (!_isPlayerDetected)
        {
            
            Vector2 moveDirection = (targetPoint.position - _transform.position).normalized; 
            float distanceToTarget = Vector2.Distance(_transform.position, targetPoint.position);
            float currentSpeed = (distanceToTarget > slowdownDistance) ? walkHorizontalSpeed :
                Mathf.Lerp(0, walkHorizontalSpeed, distanceToTarget / slowdownDistance);
            _enemyRigidbody.velocity = moveDirection * currentSpeed;
           
            
            if (distanceToTarget < minDistance)
            {
                
                return true;
            }
        }
        return false;
    }
    
}








