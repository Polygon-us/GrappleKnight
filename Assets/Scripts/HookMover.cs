using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookMover : IMovable
{
    
    
    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;

    private Vector2 _swingSpeed;
    private Vector2 _direction;

    private InputAction _inputAxisMovement;

    private TargetCameraController2 _targetCameraController;
    
    private Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>> _inputActions = 
        new Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>>();
    private float last;

    public HookMover(Rigidbody2D rigidbody2D, SpringJoint2D springJoint2D, Vector2 swingSpeed, TargetCameraController2 targetCameraController)
    {
        _rigidbody2D = rigidbody2D;
        _springJoint2D = springJoint2D;
        _swingSpeed = swingSpeed;
        _targetCameraController = targetCameraController;
        FillInputAction();
    }
    

    public void DoMove()
    {
        if (_inputAxisMovement != null && _inputAxisMovement.inProgress)
        {
            _direction = _inputAxisMovement.ReadValue<Vector2>();
        }
        else
        {
            _direction = Vector2.zero;
        }
        if (_direction.x != last)
        {
            last = _direction.x;
            _targetCameraController.MoveCameraPosition(last, 4);
        }
        _rigidbody2D.AddForce(new Vector2(_direction.x,0)*_swingSpeed.x,ForceMode2D.Impulse);
        if (_springJoint2D.distance<10)
        {
            _springJoint2D.distance += -1*_direction.y * _swingSpeed.y;
        }
        else
        {
            if (_direction.y>0)
            {
                _springJoint2D.distance += -1*_direction.y * _swingSpeed.y;
            }
        }
    }
    public void HorizontalAndVerticalInput(InputAction.CallbackContext callbackContext)
    {
        _inputAxisMovement = callbackContext.action;
    }

    private void FillInputAction()
    {
        _inputActions.Add(PlayerInputEnum.Movement,HorizontalAndVerticalInput);
    }

    public Action<InputAction.CallbackContext> GetAction(PlayerInputEnum playerInputEnum)
    {
        Action<InputAction.CallbackContext> inputAction = _inputActions[playerInputEnum];
        return inputAction;
    }
}