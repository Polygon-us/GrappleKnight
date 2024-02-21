using System;
using UnityEngine.InputSystem;

public interface IMovable
{
    void DoMove();
    Action<InputAction.CallbackContext> GetAction(PlayerInputTypeEnum playerInputTypeEnum);
}