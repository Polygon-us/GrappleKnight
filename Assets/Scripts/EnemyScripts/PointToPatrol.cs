using UnityEngine;

public class PointToPatrol : MonoBehaviour
{
    [Header("PointToPatrol")]
    [SerializeField] private float waitTime = 1f; 

    [Header("Movement")]
    [SerializeField] private float walkHorizontalSpeed = 2f;
    [SerializeField] private float minDistance = 0.1f; 
    [SerializeField] private float slowdownDistance = 1f; 
    private bool isWaiting = false; 

    [Header("References")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Transform targetPoint;

    void Start()
    {
        SetTargetPoint(pointA);
    }

    void Update()
    {
        if (isWaiting)
            return;

        MoveToTargetPoint();
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

        transform.Translate(moveDirection * currentSpeed * Time.deltaTime);

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

    System.Collections.IEnumerator WaitBeforeNextPoint()
    {
        yield return new WaitForSeconds(waitTime);

        SetTargetPoint((targetPoint == pointA) ? pointB : pointA);

        isWaiting = false;
    }
}








