using UnityEngine;
using UnityEngine.InputSystem;

public class BoomerangSkill : ISkill
{
    private Transform _boomerangTransform;
    private Transform _transform;

    private Vector3 _newPosition;
    private Vector3 _positionOnThrow;
    
    private float _maxDistanceBoomerang;
    
    private Camera _mainCamera;
    private Mouse _mousePosition;

    private float? _skillDuration = null;
    private bool _isReturn;
    public BoomerangSkill(Transform transform, Transform boomerangTransform, float maxDistanceBoomerang)
    {
        _transform = transform;
        _boomerangTransform = boomerangTransform;
        _maxDistanceBoomerang = maxDistanceBoomerang;
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
            
            _skillDuration ??= 0;
            _isReturn = false;
        }
        if (!_isReturn)
        {
            _skillDuration += Time.deltaTime;
            _boomerangTransform.position = Vector3.Lerp(_positionOnThrow,_newPosition,(float)_skillDuration);
            if (_skillDuration>=1)
            {
                _isReturn = true;
            }
        }
        else
        {
            _skillDuration -= Time.deltaTime;
            _boomerangTransform.position = Vector3.Lerp(_transform.position,_newPosition,(float)_skillDuration);
        }
        if (_skillDuration<=0)
        {
            _boomerangTransform.localPosition = Vector3.zero;
            _skillDuration = null;
            return false;
        }
        Debug.Log("Lanzar boomerang");
        return true;
    }

    public void UndoSkill()
    {
       
    }
}