using UnityEngine;
using UnityEngine.InputSystem;

public class HookSkill : ISkill
{
    private Camera _mainCamera;
    private Mouse _mousePosition;

    private Transform _transform;
    private Transform _hookBegin;
    private Transform _hookEnd;

    private SpringJoint2D _springJoint2D;

    private float _hookMaxDistance;
    private LayerMask _hookLayerMask;
    public HookSkill(Transform transform,Transform hookEnd,Transform hookBegin, SpringJoint2D springJoint2D
        ,float hookMaxDistance, LayerMask hookLayerMask)
    {
        _transform = transform;
        _hookBegin = hookBegin;
        _hookEnd = hookEnd;
        _hookMaxDistance = hookMaxDistance;
        _hookLayerMask = hookLayerMask;
        _mainCamera = Camera.main;
        _mousePosition = Mouse.current;
        _springJoint2D = springJoint2D;
        _springJoint2D.anchor = hookBegin.localPosition;
    }

    public bool DoSkill()
    {
        Vector3 newPosition = _mousePosition.position.ReadValue();
        newPosition = _mainCamera.ScreenToWorldPoint(newPosition);
        newPosition.z = 0;
        
        Vector3 createPosition = newPosition - _hookBegin.position;
        
        RaycastHit2D hit = Physics2D.Raycast(_hookBegin.position, createPosition, _hookMaxDistance);
        Physics2D.Raycast( _hookBegin.position, createPosition, _hookMaxDistance,_hookLayerMask);
        
        if (hit.collider != null)
        {
            _hookEnd.parent = null;
            _springJoint2D.enabled = true;
            _hookEnd.position = hit.point;
        }

        return false;
    }

    public void UndoSkill()
    {
        _hookEnd.parent = _transform;
       _springJoint2D.enabled = false;
    }
}