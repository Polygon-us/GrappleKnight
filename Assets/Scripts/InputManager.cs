using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager 
{
   private PlayerInputAction _playerInputAction;
   private InputAction _scrollInputAction;
   private InputAction _throwSkillnputAction;

   private Action<InputAction.CallbackContext> _changeSkillReceiver;
   private Action<InputAction.CallbackContext> _throwSkillReceiver;
   private Action<InputAction.CallbackContext> _cancelSkillReceiver;
   public InputManager(PlayerInputAction playerInputAction, Action<InputAction.CallbackContext> changeSkillReceiver, 
      Action<InputAction.CallbackContext> throwSkillReceiver, Action<InputAction.CallbackContext> cancelSkillReceiver)
   {
      _playerInputAction = playerInputAction;
      _changeSkillReceiver = changeSkillReceiver;
      _throwSkillReceiver = throwSkillReceiver;
      _cancelSkillReceiver = cancelSkillReceiver;
   }

   public void Configure()
   {
      _scrollInputAction = _playerInputAction.PlayerSkillActionMap.ChangeSkill;
      _throwSkillnputAction = _playerInputAction.PlayerSkillActionMap.ThrowSkill;
      _scrollInputAction.started += _changeSkillReceiver;
      _scrollInputAction.Enable();
      _throwSkillnputAction.started += _throwSkillReceiver;
      _throwSkillnputAction.canceled += _cancelSkillReceiver;
      _throwSkillnputAction.Enable();
   } 
   public void UnsubscribeActions()
   {
      _scrollInputAction.started -= _changeSkillReceiver;
      _scrollInputAction.Disable();
      _throwSkillnputAction.started -= _throwSkillReceiver;
      _throwSkillnputAction.canceled -= _cancelSkillReceiver;
      _throwSkillnputAction.Disable();

   }
}