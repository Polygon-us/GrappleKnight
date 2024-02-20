using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputManager
{
   private PlayerInputAction _playerInputAction;
   
   private InputAction _scrollInputAction;
   private InputAction _throwSkillInputAction;
   private InputAction _movementInputAction;

   private Action<InputAction.CallbackContext> _changeSkillReceiver;
   private Action<InputAction.CallbackContext> _throwSkillReceiver;
   private Action<InputAction.CallbackContext> _cancelSkillReceiver;
   private Action<InputAction.CallbackContext> _movementReceiver;

 
   public InputManager(PlayerInputAction playerInputAction, Action<InputAction.CallbackContext> changeSkillReceiver, 
      Action<InputAction.CallbackContext> throwSkillReceiver, Action<InputAction.CallbackContext> cancelSkillReceiver,
      Action<InputAction.CallbackContext> movementReceiver)
   {
      _playerInputAction = playerInputAction;
      
      _changeSkillReceiver = changeSkillReceiver;
      _throwSkillReceiver = throwSkillReceiver;
      _cancelSkillReceiver = cancelSkillReceiver;
      _movementReceiver = movementReceiver;
      
      _scrollInputAction = _playerInputAction.PlayerSkillActionMap.ChangeSkill;
      _throwSkillInputAction = _playerInputAction.PlayerSkillActionMap.ThrowSkill;
      _movementInputAction = _playerInputAction.PlayerHookMovement.Movement;
   }

   public void Configure()
   {
      _movementInputAction.performed += _movementReceiver;
      _movementInputAction.Enable();    
      _scrollInputAction.started += _changeSkillReceiver;
      _scrollInputAction.Enable();
      _throwSkillInputAction.started += _throwSkillReceiver;
      _throwSkillInputAction.canceled += _cancelSkillReceiver;
      _throwSkillInputAction.Enable();
   }
   
   public void UnsubscribeActions()
   {
      _movementInputAction.performed -= _movementReceiver;
      _movementInputAction.Disable();    
      _scrollInputAction.started -= _changeSkillReceiver;
      _scrollInputAction.Disable();
      _throwSkillInputAction.started -= _throwSkillReceiver;
      _throwSkillInputAction.canceled -= _cancelSkillReceiver;
      _throwSkillInputAction.Disable();

   }
}