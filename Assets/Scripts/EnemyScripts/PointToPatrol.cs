using Unity.Mathematics;
using UnityEngine;

public class PointToPatrol : MonoBehaviour
{
    [Header("PointToPatrol")]

    [SerializeField] private RaycastHit2D _pointToPatrolA;
    [SerializeField] private RaycastHit2D _pointToPatrolB;
    [SerializeField] private float _raycastLength = 0.045f;
    [SerializeField] private Vector2 point = Vector2.down;
    [SerializeField] private LayerMask _floorLayer;


    [Header("Movement")]

    [SerializeField] private float walkHorizontalSpeed = 4f;

    [Header("References")]

    [SerializeField] private Transform _transformA;
    [SerializeField] private Transform _transformB;

    void Start()
    {
    }

    void Update()
    {
        LaunchPatrolPointA();
        LaunchPatrolPointB();
    }

    private void HorizontalMovement()
    {

        transform.Translate(walkHorizontalSpeed * Time.fixedDeltaTime * Vector2.right);

    }

    public void LaunchPatrolPointA()
    {
        _pointToPatrolA = Physics2D.Raycast(_transformA.position, point, _raycastLength, _floorLayer);
        if (_pointToPatrolA.collider)
        {
            Vector2 collisionPoint = _pointToPatrolA.point;
            Debug.Log("PUNTO A " + collisionPoint);
        }
        Debug.DrawRay(_transformA.position, _transformA.TransformDirection(point * _raycastLength), Color.red);
    }
    public void LaunchPatrolPointB()
    {
        _pointToPatrolB = Physics2D.Raycast(_transformB.position, point, _raycastLength);
        
        if (_pointToPatrolB.collider)
        {
            Vector2 collisionPoint = _pointToPatrolB.point;
            Debug.Log("PUNTO B " + collisionPoint);
        }
        Debug.DrawRay(_transformB.position, _transformB.TransformDirection(point * _raycastLength), Color.red);
    }
}
