using UnityEngine;
using UnityEngine.InputSystem;

public class BoomerangSkill : ISkill
{
 
    private Transform _boomerangTransform;
    private ObjectBoomerang _objectBoomerang;
    private Transform _transform;

    private Vector3 _newPosition;
    private Vector3 _createPosition;
    private Vector3 _positionOnThrow;
    private Vector3 _initialPosition;
    
    private float _boomerangMaxDistance;
    private float _boomerangSpeed;
    
    private Camera _mainCamera;
    private Mouse _mousePosition;

    private float _skillDuration;
    public BoomerangSkill(Transform transform, Transform boomerangTransform, float boomerangMaxDistance, float boomerangSpeed)
    {
        
        _transform = transform;
        _boomerangTransform = boomerangTransform;
        _boomerangMaxDistance = boomerangMaxDistance;
        _boomerangSpeed = boomerangSpeed;
        _mainCamera = Camera.main;
        _mousePosition = Mouse.current;
        InitObjectBoomerang();
    }
    
    private void InitObjectBoomerang()
    {
        _objectBoomerang = _boomerangTransform.GetComponent<ObjectBoomerang>();
        _objectBoomerang.Init(BoomerangCollision);
    }

    private void BoomerangCollision()
    {
        if (_skillDuration<0)
        {
            _skillDuration *= -1;
        }
    }
    public void InitSkill()
    {
        _newPosition = _mousePosition.position.ReadValue();
        _newPosition = _mainCamera.ScreenToWorldPoint(_newPosition);

        _newPosition.z = 0;
        
        _positionOnThrow = _transform.position;

        Vector3 restPositions = _newPosition - _positionOnThrow;
        restPositions = restPositions.normalized*_boomerangMaxDistance;
        _newPosition = _positionOnThrow + restPositions;
        _initialPosition = _positionOnThrow;
        _skillDuration = -1;
        _boomerangTransform.localPosition = Vector3.zero;
        _boomerangTransform.gameObject.SetActive(true);
    }

    public bool DoSkill()
    {
        _skillDuration += _boomerangSpeed*Time.deltaTime;
        float t = -(_skillDuration * _skillDuration) + 1;
        _boomerangTransform.position = Vector3.Lerp(_initialPosition, _newPosition, t);
        if (_skillDuration>=0)
        {
            _initialPosition = _transform.position;
        }
        if (t<0)
        {
            _boomerangTransform.gameObject.SetActive(false);
            return false;
        }
        return true;
    }
    
    public void UndoSkill()
    {
       
    }

    public PlayerMovementEnum SendActionMapTypeEnum()
    {
        return PlayerMovementEnum.None;
    }

    public void UnsubscribeActions()
    {
        _objectBoomerang.UnsubscribeAction();
    }
    
}