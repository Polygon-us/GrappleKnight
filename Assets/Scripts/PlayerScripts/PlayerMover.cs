using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : IMovable
{
    private float _horizontalSpeed;
    private float _horizontalForceOnGround;
    private float _horizontalForceOnAir;
    private float _jumpHeight;
    private float _raycastLength;
    

    private LayerMask _checkFloorMask;
    
    private RaycastHit2D _checkFloor;
    
    private Vector2 _moveAxis;
    
    private Rigidbody2D _myrygidbody;
    private Transform _playerTransform;

    private bool _isJumpOnAir;

    private float _currentLastJump = 1;
    
    private InputAction _inputAxisMovement;
    

    private Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>> _inputActions = 
        new Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>>();

    private float _maxTimeLastJump = 0.2f;
    private float _velocity;

    public PlayerMover(Transform playerTransform,Rigidbody2D myrygidbody,float horizontalSpeed,float horizontalForceOnGround, float horizontalForceOnAir,
        float jumpHeight, float raycastLength ,LayerMask checkFloorMask)
    {
        _playerTransform = playerTransform;
        _myrygidbody = myrygidbody;
        _horizontalSpeed = horizontalSpeed;
        _horizontalForceOnGround = horizontalForceOnGround;
        _horizontalForceOnAir = horizontalForceOnAir;
        _jumpHeight = jumpHeight;
        _raycastLength = raycastLength;
        _checkFloorMask = checkFloorMask;
        _velocity = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _myrygidbody.gravityScale)*-2)*_myrygidbody.mass;
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
        Jump();
    }

    public Action<InputAction.CallbackContext> GetAction(PlayerInputEnum playerInputEnum)
    {
        Action<InputAction.CallbackContext> inputAction = _inputActions[playerInputEnum];
        return inputAction;
    }
    private void HorizontalMovement()
    {
        
        if (_moveAxis != Vector2.zero )
        {
            if (OnGround())
            {
                _myrygidbody.AddForce(Vector2.right*_moveAxis.x*_horizontalForceOnGround);
            }
            else
            {
                _myrygidbody.AddForce(Vector2.right*_moveAxis.x*_horizontalForceOnAir);
            }
            _myrygidbody.velocity = new Vector2(Mathf.Clamp(_myrygidbody.velocity.x,-_horizontalSpeed, 
                _horizontalSpeed), _myrygidbody.velocity.y);
        }
        else
        {
            if (OnGround())
            {
                _myrygidbody.velocity = new Vector2(0, _myrygidbody.velocity.y);
            }
        }
    }
            
    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (OnGround())
        {
            _myrygidbody.AddForce(Vector2.up * _velocity, ForceMode2D.Impulse);
            return;
        }
        _currentLastJump = 0;
    } 
    private void Jump()
    {
        _currentLastJump += Time.deltaTime;
        if (OnGround() && _currentLastJump <= _maxTimeLastJump )
        {
            _myrygidbody.velocity = new Vector2(_myrygidbody.velocity.x, 0);
            _myrygidbody.AddForce(Vector2.up * _velocity, ForceMode2D.Impulse);
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