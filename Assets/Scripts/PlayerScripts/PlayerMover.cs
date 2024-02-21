using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : IMovable
{
    private Transform _playerTransform;
    private float _horizontalSpeed;

    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private LayerMask checkFloorMask;
    private float raycastLength = 1.01f;
    private RaycastHit2D _checkFloor;
    private Rigidbody2D _myrygidbody;
    
    private Vector2 _moveAxis;

    private Dictionary<PlayerInputTypeEnum, Action<InputAction.CallbackContext>> _inputActions;

    private void FillInputAction()
    {
        _inputActions.Add(PlayerInputTypeEnum.Movement,HorizontalInput);
        _inputActions.Add(PlayerInputTypeEnum.Jump,JumpInput);
    }
    public PlayerMover(Transform playerTransform, float horizontalSpeed)
    {
        _playerTransform = playerTransform;
        _horizontalSpeed = horizontalSpeed;
        FillInputAction();
    }

    public void DoMove(InputAction inputActionMovement)
    {
        HorizontalMovement();
        Jump();
        OnGround();
    }

    public Action<InputAction.CallbackContext> GetAction(PlayerInputTypeEnum playerInputTypeEnum)
    {
        Action<InputAction.CallbackContext> inputAction = _inputActions[playerInputTypeEnum];
        return inputAction;
    }

    private void HorizontalMovement()
    {
        _playerTransform.Translate(new Vector2(_moveAxis.x,0) * Time.fixedDeltaTime * _horizontalSpeed);
    }
    public void Jump()
    {
        if (_checkFloor.collider != null && Input.GetKeyDown(KeyCode.Space))
        {

            _myrygidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void OnGround()
    {
        _checkFloor = Physics2D.Raycast(_playerTransform.position, Vector2.down, raycastLength, checkFloorMask);

        Debug.DrawRay(_playerTransform.position, _playerTransform.TransformDirection(Vector3.down * raycastLength), Color.red);
    }
    
    public void HorizontalInput(InputAction.CallbackContext callbackContext)
    {
        callbackContext.ReadValue<Vector2>();
    }

    public void JumpInput(InputAction.CallbackContext callbackContext)
    {
        
    }
    
}