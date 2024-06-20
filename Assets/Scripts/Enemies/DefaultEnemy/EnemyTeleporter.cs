using System.Collections;
using UnityEngine;

public class EnemyTeleporter : MonoBehaviour
{
    private float _time;

    private Vector3 _inicialPosition;
    private Vector3 _pointA;
    
    private void Awake()
    {
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
        float distanceOfY = Mathf.Abs(_pointA.y - transform.position.y);

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
            transform.position = _inicialPosition;
        }
       
    }
}
