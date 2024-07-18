using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerClimbingMover : IMovable
{
    [Header("isClimbing")]

    private float _climbingSpeed = 4f;
    private bool isClimbing;
    private LayerMask checkLimitStair;
    private CapsuleCollider2D _myCapsuleCollider;
    private PlatformEffector2D _platformEffector;


    private float _inicialGravity;
    private RaycastHit2D _checkStairLimit;
    private Vector2 _verticalMovement;

    [Header("References")]

    private Transform _myTransform;
    private Rigidbody2D _myrygidbody;
    private BoxCollider2D _boxColliderStairStop;

    private InputAction _inputAxisMovement;

    private bool _movementEnabled = true;
    private RigidbodyConstraints2D _originalConstraints;
    
    public PlayerClimbingMover(float climbingSpeed, Transform myTransform, Rigidbody2D myrigidbody)
    {
        _climbingSpeed = climbingSpeed;
        _myTransform = myTransform;
       _myrygidbody = myrigidbody;
       _originalConstraints = myrigidbody.constraints;
    }

    private void VerticalMovement()
    {
        
        if ((_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Stair"))))
        {
            _myTransform.Translate(_climbingSpeed * Time.fixedDeltaTime * Vector2.up * _verticalMovement);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stair"))
        {
            _platformEffector = other.transform.Find("StairTop").GetComponent<PlatformEffector2D>();
            _boxColliderStairStop = other.transform.Find("StairTop").GetComponent<BoxCollider2D>();
        }
    }
    public void Climbing()
    {
        if ((_verticalMovement.y != 0 || isClimbing) && (_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Stair"))))
        {
            _myrygidbody.gravityScale = 0;
            isClimbing = true;
            Debug.Log("cero gravedad");
        }
        else
        {
            _myrygidbody.gravityScale = _inicialGravity;
            isClimbing = false;
        }
        if ((_verticalMovement.y < 0) && (_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("StairStop"))))
        {
            _boxColliderStairStop.enabled = false;
        }
        else if ((_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Floor"))))
        {
            _platformEffector.enabled = true;
            _boxColliderStairStop.enabled = true;
        }

    }

    public void DoMove()
    {
        if(!_movementEnabled) return;
        
        if (_inputAxisMovement != null && _inputAxisMovement.inProgress)
        {
            _verticalMovement = _inputAxisMovement.ReadValue<Vector2>();
        }
        else
        {
            _verticalMovement = Vector2.zero;
        }
        Climbing();
        VerticalMovement();
    }

    public void EnableMovement(bool canMove)
    {
        _movementEnabled = canMove;
        
        _myrygidbody.constraints = canMove ? _originalConstraints : RigidbodyConstraints2D.FreezeAll;
    }

    private void VerticalInput(InputAction.CallbackContext callbackContext)
    {
        _inputAxisMovement = callbackContext.action;
    }

    public Action<InputAction.CallbackContext> GetAction(PlayerInputEnum playerInputEnum)
    {
        return VerticalInput;
    }
}
