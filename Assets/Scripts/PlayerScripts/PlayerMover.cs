using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : IMovable
{

    private float _jumpHeight;
    private float _jumpSpeed;
    private float _raycastLength;
    private float _maxSpeed;
    private float _maxAcceleration;
    private float _maxAirAcceleration;
    private float _maxSpeedChange, _acceleration;
   
    private Vector2 _direction, _desiredVelocity, _velocity;
    private Vector2 _moveAxis;

    public bool _onGround;

    private LayerMask _checkFloorMask;
    
    private RaycastHit2D _checkFloor;
    
    
    private Rigidbody2D _myRigidbody;
    private Transform _playerTransform;

    private bool _isJumpOnAir;

    private float _currentLastJump = 1;
    private float _maxTimeLastJump = 0.2f;
    
    private InputAction _inputAxisMovement;
    
    public Action<InputAction> OnInputMoveChange;

    private Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>> _inputActions = 
        new Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>>();

    public PlayerMover(Transform playerTransform,Rigidbody2D myRigidbody,float maxSpeed, float maxAcceleration, float jumpHeight,
        float raycastLength ,LayerMask checkFloorMask, float maxAirAcceleration)
    {
        _playerTransform = playerTransform;
        _myRigidbody = myRigidbody;
        _maxSpeed = maxSpeed;
        _maxAcceleration = maxAcceleration;
        _jumpHeight = jumpHeight;
        _raycastLength = raycastLength;
        _checkFloorMask = checkFloorMask;
        _maxAirAcceleration = maxAirAcceleration;
        _jumpSpeed = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _myRigidbody.gravityScale) * -2) * _myRigidbody.mass;
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

            _desiredVelocity = new Vector2(_moveAxis.x, 0f) * Mathf.Max(_maxSpeed , 0f);
            _velocity = _myRigidbody.velocity;

            _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
            
            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _myRigidbody.velocity = _velocity;

        }
        else
        {
            if (OnGround())
            {
                _onGround = true;
                _myRigidbody.velocity = new Vector2(0, _myRigidbody.velocity.y);
            }
            else if (!OnGround())
            {
                _onGround = false;
            }
            
        }
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (OnGround())
        {
            _myRigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            return;
        }
        _currentLastJump = 0;
    } 
    private void Jump()
    {
        _currentLastJump += Time.deltaTime;
        if (OnGround() && _currentLastJump <= _maxTimeLastJump )
        {
            _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, 0);
            _myRigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
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
        OnInputMoveChange?.Invoke(_inputAxisMovement);
    }
    
}