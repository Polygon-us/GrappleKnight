using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeMover : MonoBehaviour
{
    private Transform _hookBegin;
    private float _hookMaxDistance;
    
    private Camera _mainCamera;
    private Mouse _mousePosition;
    
    private Vector3 newPosition;
    private Vector3 _outPoint;
    private Vector3 _createPosition;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _mousePosition = Mouse.current;
    }

    public void Configure(Transform hookBegin, float hookMaxDistance)
    {
        Cursor.visible = false;
        
        _hookBegin = hookBegin;
        _hookMaxDistance = hookMaxDistance;
        transform.parent = null;
    }
    private void Update()
    {
        newPosition = _mousePosition.position.ReadValue();
        newPosition = _mainCamera.ScreenToWorldPoint(newPosition);
        newPosition.z = 0;
        _createPosition = newPosition - _hookBegin.position;
        float distance = (_hookBegin.position.x-newPosition.x) * (_hookBegin.position.x-newPosition.x) +
                         (_hookBegin.position.y-newPosition.y) * (_hookBegin.position.y-newPosition.y);
        if (distance<=_hookMaxDistance*_hookMaxDistance)
        {
            _outPoint = newPosition;
        }
        else
        {
            _outPoint = _createPosition.normalized*_hookMaxDistance +_hookBegin.position;
        }
        transform.position = _outPoint;
    }
}