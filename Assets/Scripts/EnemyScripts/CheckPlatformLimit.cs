using System.Collections;
using UnityEngine;

public class VerifyPlataform : MonoBehaviour
{


    [Header("Raycast")]

    [SerializeField] private RaycastHit2D _platformDetector;
    [SerializeField] private LayerMask _floorMask;
    private float _time;
    private float _raycastLength = 1.02f;
    private Vector3 _inicialPosition;
    [Header("References")]

     private Transform _enemy;

    private void Start()
    {
            _enemy = transform;
            _inicialPosition = transform.position;
    }
    private void FixedUpdate()
    {
        DetecPlatform();
    }
    void Update()
    {
        IfNotDetectPlatform();
    }

    public void DetecPlatform()
    {
        _platformDetector = Physics2D.Raycast(_enemy.position, Vector2.down, _raycastLength, _floorMask);
        Debug.DrawRay(_enemy.position, Vector2.down * _raycastLength, Color.blue);
    }
    public void IfNotDetectPlatform() 
    {
        if (!_platformDetector)
        {
            
            InitTimeToRespawn();
        }
    }

    public void InitTimeToRespawn () 
    {
        _time += Time.deltaTime;

        if (_time > 5f)
        {
            _enemy.position = _inicialPosition;
        }
    }

}
