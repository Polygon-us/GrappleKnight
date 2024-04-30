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
    private float mover;
    private float _maxTimeLastJump = 0.2f;
    private float _currentLastJump = 1;
    private float lastMove;

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;
    private Vector2 _moveAxis;

    private bool _isStop;
    private bool _onGround;
    private bool _isJumpOnAir;
    private bool _isOnMove;
    
    private LayerMask _checkFloorMask;
    
    private RaycastHit2D _checkFloor;
    
    private Rigidbody2D _floorTouching;
    private Transform _playerTransform;
    
    private InputAction _inputAxisMovement;
    public PlayerClimbing _playerClimbing;
    private TargetCameraController2 _targetCameraController;
    
    public Action<InputAction> OnInputMoveChange;

    private Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>> _inputActions = 
        new Dictionary<PlayerInputEnum, Action<InputAction.CallbackContext>>();

    public PlayerMover(Transform playerTransform,Rigidbody2D myRigidbody,float maxSpeed, float maxAcceleration, float jumpHeight,
        float raycastLength ,LayerMask checkFloorMask, float maxAirAcceleration, Vector2 moveAxis, TargetCameraController2 targetCameraController)
    {
        _playerTransform = playerTransform;
        _floorTouching = myRigidbody;
        _maxSpeed = maxSpeed;
        _maxAcceleration = maxAcceleration;
        _jumpHeight = jumpHeight;
        _raycastLength = raycastLength;
        _checkFloorMask = checkFloorMask;
        _maxAirAcceleration = maxAirAcceleration;
        _moveAxis = moveAxis;
        _jumpSpeed = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _floorTouching.gravityScale) * -2) * _floorTouching.mass;
        _targetCameraController = targetCameraController;
        FillInputAction();
    }
   
    private void FillInputAction()
    {
        InputManagerTwo.movementMap.Movement.performed += HorizontalInput;
        InputManagerTwo.movementMap.Jump.performed += Jump;
        
        // _inputActions.Add(PlayerInputEnum.Movement,HorizontalInput);
        // _inputActions.Add(PlayerInputEnum.Jump,Jump);
    }

    public void DoMove()
    {
        if (_inputAxisMovement != null && _inputAxisMovement.inProgress)
        {
            _moveAxis = _inputAxisMovement.ReadValue<Vector2>();
             mover = _moveAxis.x;
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
            if(_moveAxis.x != lastMove)
            {
                lastMove = _moveAxis.x;
                _targetCameraController.MoveCameraPosition(lastMove,8);
            }
            _desiredVelocity = new Vector2(_moveAxis.x, 0f) * Mathf.Max(_maxSpeed , 0f);
            _velocity = _floorTouching.velocity;

            _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;

            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
            
            _floorTouching.velocity = _velocity;
            _isStop = false;
        }
        else
        {
            if (!_isStop)
            {
                if (OnGround())
                {
                     _onGround = true;
                    _floorTouching.velocity = new Vector2(0, _floorTouching.velocity.y);
                    _isStop = true;
                }
                else
                {
                    _onGround = false;
                }
            }
        }
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (OnGround() )
        {
                _floorTouching.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            return;
        }
        _currentLastJump = 0;
    } 
    private void Jump()
    {
        _currentLastJump += Time.deltaTime;

        if (OnGround() && _currentLastJump <= _maxTimeLastJump )
        {
                _floorTouching.velocity = new Vector2(_floorTouching.velocity.x, 0);
            _floorTouching.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
        
    }
    private bool OnGround()
    {   
        if (_floorTouching.IsTouchingLayers(LayerMask.GetMask("Floor")))
        {
            return true;
        }
        return false;

        //_checkFloor = Physics2D.Raycast(_playerTransform.position, Vector2.down, _raycastLength, _checkFloorMask);
        //Debug.DrawRay(_playerTransform.position, -_playerTransform.up*_raycastLength, Color.red);
        //if (_checkFloor.collider != null)
        //{

        //    return true;
        //}

        //return false;
    }
    
    private void HorizontalInput(InputAction.CallbackContext callbackContext)
    {
        _inputAxisMovement = callbackContext.action;
        OnInputMoveChange?.Invoke(_inputAxisMovement);
    }
}