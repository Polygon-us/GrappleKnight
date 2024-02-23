using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : IMovable
{
    private Transform _playerTransform;
    private float _horizontalSpeed;
    private float _jumpForce;
    private LayerMask _checkFloorMask;
    private float _raycastLength;
    private Rigidbody2D _myrygidbody;
    
    private RaycastHit2D _checkFloor;
    
    private Vector2 _moveAxis;
    
    private InputAction _inputAxisMovement;

    private Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>> _inputActions = 
        new Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>>();

    public PlayerMover(Transform playerTransform,Rigidbody2D myrygidbody,float horizontalSpeed,float jumpForce,float raycastLength ,LayerMask checkFloorMask)
    {
        _playerTransform = playerTransform;
        _myrygidbody = myrygidbody;
        _horizontalSpeed = horizontalSpeed;
        _jumpForce = jumpForce;
        _raycastLength = raycastLength;
        _checkFloorMask = checkFloorMask;
        FillInputAction();
    }
    
    private void FillInputAction()
    {
        _inputActions.Add(PlayerInputEnum.Movement,HorizontalInput);
        _inputActions.Add(PlayerInputEnum.Jump,Jump);
    }

    public void DoMove()
    {
        if (_inputAxisMovement != null && _inputAxisMovement.inProgress)
        {
            _moveAxis = _inputAxisMovement.ReadValue<Vector2>();
        }
        else
        {
            _moveAxis = Vector2.zero;
        }
        HorizontalMovement();
    }

    public Action<InputAction.CallbackContext> GetAction(PlayerInputEnum playerInputEnum)
    {
        Action<InputAction.CallbackContext> inputAction = _inputActions[playerInputEnum];
        return inputAction;
    }

    private void HorizontalMovement()
    {
        _myrygidbody.AddForce(new Vector2(_moveAxis.x,0) * _horizontalSpeed, ForceMode2D.Impulse);
        //a_playerTransform.Translate(new Vector2(_moveAxis.x,0) * Time.fixedDeltaTime * _horizontalSpeed);
    }
    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (OnGround())
        {
            _myrygidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
    private bool OnGround()
    {
        _checkFloor = Physics2D.Raycast(_playerTransform.position, Vector2.down, _raycastLength, _checkFloorMask);
        Debug.DrawRay(_playerTransform.position, -_playerTransform.up*_raycastLength, Color.red);
        if (_checkFloor.collider != null)
        {
            return true;
        }

        return false;
    }
    
    private void HorizontalInput(InputAction.CallbackContext callbackContext)
    {
        _inputAxisMovement = callbackContext.action;
    }
    
}