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

    // Añadir referencias al Animator y SpriteRenderer
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _movementEnabled = true;
    private RigidbodyConstraints2D _originalConstraints;

    public PlayerMover(Transform playerTransform, Rigidbody2D myRigidbody, float maxSpeed, float maxAcceleration, float jumpHeight,
        float raycastLength, LayerMask checkFloorMask, float maxAirAcceleration, Vector2 moveAxis, TargetCameraController2 targetCameraController, SpriteRenderer spriteRenderer)
    {
        _playerTransform = playerTransform;
        _floorTouching = myRigidbody;
        _originalConstraints = myRigidbody.constraints;
        _maxSpeed = maxSpeed;
        _maxAcceleration = maxAcceleration;
        _jumpHeight = jumpHeight;
        _raycastLength = raycastLength;
        _checkFloorMask = checkFloorMask;
        _maxAirAcceleration = maxAirAcceleration;
        _moveAxis = moveAxis;
        _jumpSpeed = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _floorTouching.gravityScale) * -2) * _floorTouching.mass;
        _targetCameraController = targetCameraController;

        // Obtener referencias a los componentes Animator y SpriteRenderer
        _animator = playerTransform.GetComponentInChildren<Animator>();
        _spriteRenderer = spriteRenderer;
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
        if (!_movementEnabled)
        {
            _animator.SetFloat("Speed", 0f);
            return;
        }
        
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
    }

    public void EnableMovement(bool canMove = true)
    {
        _movementEnabled = canMove;

        _floorTouching.constraints = canMove ? _originalConstraints : RigidbodyConstraints2D.FreezeAll;
    }
    
    public Action<InputAction.CallbackContext> GetAction(PlayerInputEnum playerInputEnum)
    {
        Action<InputAction.CallbackContext> inputAction = _inputActions[playerInputEnum];
        return inputAction;
    }

    private void HorizontalMovement()
    {
        if (_moveAxis != Vector2.zero)
        {
            if (_moveAxis.x != lastMove)
            {
                lastMove = _moveAxis.x;
                _targetCameraController.MoveCameraPosition(lastMove, 8);
            }
            _desiredVelocity = new Vector2(_moveAxis.x, 0f) * Mathf.Max(_maxSpeed, 0f);
            _velocity = _floorTouching.velocity;

            _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;

            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _floorTouching.velocity = _velocity;
            _isStop = false;

            // Actualizar la velocidad en el Animator
            _animator.SetFloat("Speed", Mathf.Abs(_velocity.x));

            // Flipping del sprite según la dirección del movimiento
            if (_velocity.x > 0.01f)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_velocity.x < -0.01f)
            {
                _spriteRenderer.flipX = true;
            }
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

                    // Detener la animación de caminar
                    _animator.SetFloat("Speed", 0f);
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
        if (!_movementEnabled) return;
        
        if (OnGround())
        {
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
