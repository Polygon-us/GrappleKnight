using UnityEngine;
using UnityEngine.InputSystem;

public class BoomerangSkill : ISkill
{
    private Transform _boomerangTransform;
    private Transform _transform;

    private Vector3 _newPosition;
    private Vector3 _createPosition;
    private Vector3 _positionOnThrow;
    
    private float _boomerangMaxDistance;
    
    private Camera _mainCamera;
    private Mouse _mousePosition;

    private float? _skillDuration;
    private bool _isReturn;
    public BoomerangSkill(Transform transform, Transform boomerangTransform, float boomerangMaxDistance)
    {
        _transform = transform;
        _boomerangTransform = boomerangTransform;
        _boomerangMaxDistance = boomerangMaxDistance;
        _mainCamera = Camera.main;
        _mousePosition = Mouse.current;
    }

    public bool DoSkill()
    {
        if (_skillDuration==null)
        {
            _newPosition = _mousePosition.position.ReadValue();
            _newPosition = _mainCamera.ScreenToWorldPoint(_newPosition);

            _newPosition.z = 0;
            
            _positionOnThrow = _transform.position;

            Vector3 restPositions = _newPosition - _positionOnThrow;
            restPositions = restPositions.normalized*_boomerangMaxDistance;
            _newPosition = _positionOnThrow + restPositions;
            
            _skillDuration ??= -1;
            _isReturn = false;
        }
        _skillDuration += Time.deltaTime;
        float t = -((float)_skillDuration * (float)_skillDuration) + 1;
        _boomerangTransform.position = Vector3.Lerp(_positionOnThrow, _newPosition, t);
        if (t<0)
        {
            _boomerangTransform.localPosition = Vector3.zero;
            _skillDuration = null;
            return false;
        }
        return true;
    }
    
    public void UndoSkill()
    {
       
    }
}