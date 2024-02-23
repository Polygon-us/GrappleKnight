using UnityEngine;
using System.Collections;
public class EnemyPatrolState : MonoBehaviour, IState
{
    [Header("EnemyPatrolState")]
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float minDistance = 0.1f; 
    [SerializeField] private float slowdownDistance = 1f; 

    [Space]
    [Header("Movement")]
    [SerializeField] private float walkHorizontalSpeed = 2f;
    private bool isWaiting = false;

    [Space]
    [Header("References")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Rigidbody2D _enemyRigidbody;
    private Transform targetPoint;

    void Start()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        SetTargetPoint(pointA);
    }
    public bool DoState()
    {
        if (isWaiting)
            return false;

        MoveToTargetPoint();
        return true;
    }
    void FixedUpdate() 
    {
        //if (isWaiting)
        //   return;

        // MoveToTargetPoint();
    }
    void SetTargetPoint(Transform newTargetPoint)
    {
        targetPoint = newTargetPoint;
    }

    void MoveToTargetPoint()
    {
        Vector2 moveDirection = (targetPoint.position - transform.position).normalized;

        float distanceToTarget = Vector2.Distance(transform.position, targetPoint.position);

        float currentSpeed = (distanceToTarget > slowdownDistance) ? walkHorizontalSpeed :
            Mathf.Lerp(0, walkHorizontalSpeed, distanceToTarget / slowdownDistance);
        _enemyRigidbody.velocity = moveDirection * currentSpeed;

        if (distanceToTarget < minDistance)
        {
            StartWaitingForNextPoint();
        }
    }
  
    void StartWaitingForNextPoint()
    {
        isWaiting = true;

        StartCoroutine(WaitBeforeNextPoint());
    }

    IEnumerator WaitBeforeNextPoint()
    {
        yield return new WaitForSeconds(waitTime);

        SetTargetPoint((targetPoint == pointA) ? pointB : pointA);

        isWaiting = false;
    }
}








