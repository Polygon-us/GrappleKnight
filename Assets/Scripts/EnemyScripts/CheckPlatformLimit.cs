using System.Collections;
using UnityEngine;

public class VerifyPlataform : MonoBehaviour
{


    [Header("Raycast")]

    [SerializeField] private RaycastHit2D _platformDetector;
    [SerializeField] private float _raycastLegth;
    [SerializeField] private LayerMask _floorMask;

    [Header("References")]

    [SerializeField] private Transform _targetToPlatform;
   
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
        _platformDetector = Physics2D.Raycast(_targetToPlatform.position, Vector2.down, _raycastLegth, _floorMask);
        Debug.DrawRay(_targetToPlatform.position, _targetToPlatform.TransformDirection(Vector3.down * _raycastLegth), Color.red);
    }
    public void IfNotDetectPlatform() 
    {
        if (!_platformDetector)
        {   
            RotateDireccion();
        }
    }

    public void RotateDireccion () 
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

}
