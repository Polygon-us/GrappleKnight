using UnityEngine;
using UnityEngine.InputSystem;

public class HookSkill : ISkill
{
    private Camera _mainCamera;
    private Mouse _mousePosition;

    private Transform _transform;
    private Transform _hookBegin;
    private Transform _hookEnd;

    private GameObject _rope;
    
    private SpringJoint2D _springJoint2D;
    private LineRenderer _lineRenderer;
    
    private float _hookMaxDistance;
    private float _angleOfShut;
    private LayerMask _hookMask;

    private Vector3? _outPoint;

    private float _currentTimeFakeHook ;
    private float _maxTimeFakeHook = 0.1f;
    
    private bool _onHook;
    public HookSkill(Transform transform,Transform hookEnd,Transform hookBegin, SpringJoint2D springJoint2D
        ,float hookMaxDistance, float angleOfShut,GameObject rope, LayerMask hookMask)
    {
        _transform = transform;
        _hookBegin = hookBegin;
        _hookEnd = hookEnd;
        _hookMaxDistance = hookMaxDistance;
        _angleOfShut = angleOfShut;
        _mainCamera = Camera.main;
        _mousePosition = Mouse.current;
        _hookMask = hookMask;
        _rope = rope;
        _lineRenderer = _rope.GetComponent<LineRenderer>();
        _springJoint2D = springJoint2D;
        _springJoint2D.anchor = hookBegin.localPosition;
    }
    
    public void InitSkill()
    {
        UndoSkill();
        Vector3 newPosition = _mousePosition.position.ReadValue();
        newPosition = _mainCamera.ScreenToWorldPoint(newPosition);
        newPosition.z = 0;
        
        Vector3 createPosition = newPosition - _hookBegin.position;

        float angle = Vector3.SignedAngle(createPosition, Vector3.right, -Vector3.forward);
        if (angle<0)
        {
            angle += 360;
        }
        float _startAngle = 90 - (_angleOfShut / 2f);
        float _endAngle = _startAngle + _angleOfShut;
        _startAngle = 360-(_startAngle*-1);
        if (angle<=_endAngle || angle>=_startAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(_hookBegin.position, createPosition, 
                10000,_hookMask);
            if (hit.collider != null)
            {
                if (hit.distance <= _hookMaxDistance)
                {
                    _hookEnd.parent = null;
                    _hookEnd.gameObject.SetActive(true);
                    _rope.SetActive(true);
                    _springJoint2D.enabled = true;
                    _hookEnd.position = hit.point;
                    _springJoint2D.distance = Vector3.Distance(_hookBegin.position,_hookEnd.position);
                    _onHook = true;
                }
                else
                {
                    _rope.SetActive(true);
                    _outPoint = newPosition;
                }
                
            }
        }
    }
    
    public bool DoSkill()
    {
        if (_onHook)
        {
            _lineRenderer.SetPosition(0,_hookBegin.position);
            _lineRenderer.SetPosition(1,_hookEnd.position);
            return true;
        }
        if (_outPoint != null)
        {
            _currentTimeFakeHook += Time.deltaTime;
            if (_currentTimeFakeHook<=_maxTimeFakeHook)
            {
                _lineRenderer.SetPosition(0,_hookBegin.position);
                _lineRenderer.SetPosition(1,_outPoint??=new Vector3());
                return true;
            }
            _rope.SetActive(false);
            return false;
        }
        
        return false;

    }

    public void UndoSkill()
    {
        _hookEnd.parent = _transform;
        _hookEnd.gameObject.SetActive(false);
        _rope.SetActive(false);
        _springJoint2D.enabled = false;
        _onHook = false;
        _currentTimeFakeHook = 0;
        _outPoint = null;
    }

    public PlayerMovementEnum SendActionMapTypeEnum()
    {
        return PlayerMovementEnum.HookMovement;
    }

    public void UnsubscribeActions()
    {
        
    }
    
}