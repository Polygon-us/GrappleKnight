using System;
using UnityEngine.InputSystem;

public interface IMovable
{
    void DoMove();
    void EnableMovement(bool canMove);
    Action<InputAction.CallbackContext> GetAction(PlayerInputEnum playerInputEnum);
}