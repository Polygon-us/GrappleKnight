using System;
using UnityEngine.InputSystem;

public interface IMovable
{
    void DoMove(InputAction inputActionMovement);
    Action<InputAction.CallbackContext> GetAction(PlayerInputTypeEnum playerInputTypeEnum);
}