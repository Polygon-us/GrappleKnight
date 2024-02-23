using UnityEngine;
using System.Collections;
using System;
public class EnemyPatrolState : IState
{
    [Header("EnemyPatrolState")]
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float minDistance = 0.1f; 
    [SerializeField] private float slowdownDistance = 1f; 

    [Space]
    [Header("Movement")]
    [SerializeField] private float walkHorizontalSpeed = 10f;
    private bool isWaiting = false;

    [Space]
    [Header("References")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Rigidbody2D _enemyRigidbody;
    [SerializeField] private Transform _transform;
    private Transform targetPoint;

    public EnemyPatrolState(Transform pointA, Transform pointB, Rigidbody2D enemyRigidbody, Transform transform)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        _enemyRigidbody = enemyRigidbody;
        _transform = transform;
        //SetTargetPoint(pointA);
    }
    
    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        Debug.Log("Patrol");
        //if (isWaiting)
        //    return false;
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
   
    void SetTargetPoint(Transform newTargetPoint)
    {
        targetPoint = newTargetPoint;
    }

    bool MoveToTargetPoint()
    {
        Vector2 moveDirection = (targetPoint.position - _transform.position).normalized;

        float distanceToTarget = Vector2.Distance(_transform.position, targetPoint.position);

        float currentSpeed = (distanceToTarget > slowdownDistance) ? walkHorizontalSpeed :
            Mathf.Lerp(0, walkHorizontalSpeed, distanceToTarget / slowdownDistance);
        _enemyRigidbody.velocity = moveDirection * currentSpeed;

        if (distanceToTarget < minDistance)
        {
            return true;
            //StartWaitingForNextPoint();
        }
        return false;
    }
  
    void StartWaitingForNextPoint()
    {
        isWaiting = true;

        //StartCoroutine(WaitBeforeNextPoint());
    }

    IEnumerator WaitBeforeNextPoint()
    {
        yield return new WaitForSeconds(waitTime);

        SetTargetPoint((targetPoint == pointA) ? pointB : pointA);

        isWaiting = false;
    }
}








