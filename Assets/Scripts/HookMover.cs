using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookMover : IMovable
{
    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;

    private Vector2 _swingSpeed;
    public HookMover(Rigidbody2D rigidbody2D, SpringJoint2D springJoint2D, Vector2 swingSpeed)
    {
        _rigidbody2D = rigidbody2D;
        _springJoint2D = springJoint2D;
        _swingSpeed = swingSpeed;
    }

    public void DoMove(InputAction inputActionMovement)
    {
        Vector2 direction = inputActionMovement.ReadValue<Vector2>();
        _rigidbody2D.AddForce(new Vector2(direction.x,0)*_swingSpeed.x,ForceMode2D.Impulse);
        if (_springJoint2D.distance<10)
        {
            _springJoint2D.distance += -1*direction.y * _swingSpeed.y;
        }
        else
        {
            if (direction.y>0)
            {
                _springJoint2D.distance += -1*direction.y * _swingSpeed.y;
            }
        }
    }

    public Action<InputAction.CallbackContext> GetAction(PlayerInputTypeEnum playerInputTypeEnum)
    {
        return null;
    }
}