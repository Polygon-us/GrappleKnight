using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager 
{
   private PlayerInputAction _playerInputAction;
   private InputAction _scrollInputAction;

   private Action<InputAction.CallbackContext> _changeSkillReceiver;
   public InputManager(PlayerInputAction playerInputAction, Action<InputAction.CallbackContext> changeSkillReceiver)
   {
      _playerInputAction = playerInputAction;
      _changeSkillReceiver = changeSkillReceiver;
   }

   public void Configure()
   {
      _scrollInputAction = _playerInputAction.PlayerSkillActionMap.ChangeSkill;
      _scrollInputAction.performed += _changeSkillReceiver;
      _scrollInputAction.Enable();
   }
   public void UnsuscribeActions()
   {
      _scrollInputAction.performed -= _changeSkillReceiver;
      _scrollInputAction.Disable();

   }
}