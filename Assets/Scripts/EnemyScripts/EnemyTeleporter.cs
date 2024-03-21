using System.Collections;
using UnityEngine;

public class EnemyTeleporter : MonoBehaviour
{
    [Header("Raycast")]

    [SerializeField] private RaycastHit2D _platformDetector;
    [SerializeField] private LayerMask _floorMask;

    private float _time;
    private float _raycastLength = 1.1f;
    private Vector3 _inicialPosition;

    [Header("References")]

     private Transform _enemy;

    private bool _isOnFloor;

    private Vector3 _pointA;
    
    
    private void Awake()
    {
        _enemy = transform;
        _inicialPosition = transform.position;
    }
    void Update()
    {
        DetectZoneOfPatrol();
    }

    public void Configure(Transform pointA)
    {
        _pointA = pointA.position;
    }
    
    private void DetectZoneOfPatrol()
    {
        float distanceOfY = (_pointA.x-transform.position.x)*(_pointA.x-transform.position.x)+
                            (_pointA.y-transform.position.y)*(_pointA.y-transform.position.y);
        if (distanceOfY>0.5f)
        {
            InitTimeToRespawn();
        }
        else
        {
            _time = 0;
        }
    }

    private void InitTimeToRespawn () 
    {
        _time += Time.deltaTime;

        if (_time > 3f)
        {
            _time = 0;
            _isOnFloor = true;
            _enemy.position = _inicialPosition;
        }
       
    }

}
