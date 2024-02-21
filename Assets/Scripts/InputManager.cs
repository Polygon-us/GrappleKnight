using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputManager
{
   private PlayerInputAction _playerInputAction;
   
   // private InputAction _scrollInputAction;
   // private InputAction _throwSkillInputAction;
   // private InputAction _movementInputAction;
   //
   // private Action<InputAction.CallbackContext> _changeSkillReceiver;
   // private Action<InputAction.CallbackContext> _throwSkillReceiver;
   // private Action<InputAction.CallbackContext> _cancelSkillReceiver;
   // private Action<InputAction.CallbackContext> _movementReceiver;
   // private Action<InputAction.CallbackContext> _jumpReceiver;

   private Dictionary<PlayerInputTypeEnum, InputAction> _inputActionsContainer =
      new Dictionary<PlayerInputTypeEnum, InputAction>();
   private Dictionary<InputAction, List<Action<InputAction.CallbackContext>>> _receiverActionsContainer =
       new Dictionary<InputAction, List<Action<InputAction.CallbackContext>>>();
   private Dictionary<Action<InputAction.CallbackContext>,List<int>> _statesActionsContainer =
       new Dictionary<Action<InputAction.CallbackContext>,List<int>>();

   private Action<InputAction, Action<InputAction.CallbackContext>>[] _unsubscribeActionsContainer =
      new Action<InputAction, Action<InputAction.CallbackContext>>[3]; 
   
   public InputManager(PlayerInputAction playerInputAction)
   {
      _playerInputAction = playerInputAction;
      
      // _changeSkillReceiver = changeSkillReceiver;
      // _throwSkillReceiver = throwSkillReceiver;
      // _cancelSkillReceiver = cancelSkillReceiver;
      // _movementReceiver = movementReceiver;
      //
      // _scrollInputAction = _playerInputAction.PlayerSkillActionMap.ChangeSkill;
      // _throwSkillInputAction = _playerInputAction.PlayerSkillActionMap.ThrowSkill;
      // _movementInputAction = _playerInputAction.PlayerMovement.Movement;
      FillInputActionsContainer();
   }

   private void FillInputActionsContainer()
   {
      _inputActionsContainer.Add(PlayerInputTypeEnum.ChangeSkill,_playerInputAction.PlayerSkillActionMap.ChangeSkill);
      _inputActionsContainer.Add(PlayerInputTypeEnum.ThrowSkill,_playerInputAction.PlayerSkillActionMap.ThrowSkill);
      _inputActionsContainer.Add(PlayerInputTypeEnum.Movement,_playerInputAction.PlayerMovement.Movement);
   }
   
   public void Configure()
   {
      _unsubscribeActionsContainer[0] = UnsubscribeStartedAction;
      _unsubscribeActionsContainer[1] = UnsubscribePerformedAction;
      _unsubscribeActionsContainer[2] = UnsubscribeCanceledAction;
      
      // _movementInputAction.performed += _movementReceiver;
      // _movementInputAction.Enable();    
      // _scrollInputAction.started += _changeSkillReceiver;
      // _scrollInputAction.Enable();
      // _throwSkillInputAction.started += _throwSkillReceiver;
      // _throwSkillInputAction.canceled += _cancelSkillReceiver;
      // _throwSkillInputAction.Enable();
   }

   public void SubscribeStartedAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,1);
      _inputActionsContainer[playerInputTypeEnum].started += action;
   }
   public void SubscribePerformedAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,2);
      _inputActionsContainer[playerInputTypeEnum].performed += action;
   }
   public void SubscribeCanceledAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,3);
      _inputActionsContainer[playerInputTypeEnum].canceled += action;
   }
   
   private void FillContainers(InputAction currentInputAction, Action<InputAction.CallbackContext> action, int stateNum)
   {
      if (_receiverActionsContainer.TryAdd(currentInputAction,new List<Action<InputAction.CallbackContext>>()))
      {
         _receiverActionsContainer[currentInputAction].Add(action);
      }
      else
      {
         _receiverActionsContainer[currentInputAction].Add(action);
      }
      if (_statesActionsContainer.TryAdd(action,new List<int>()))
      {
         _statesActionsContainer[action].Add(stateNum);
      }
      else
      {
         _statesActionsContainer[action].Add(stateNum);
      }
   }
   private void UnsubscribeStartedAction(InputAction inputAction,Action<InputAction.CallbackContext> action)
   {
      inputAction.started -= action;
   }
   private void UnsubscribePerformedAction(InputAction inputAction,Action<InputAction.CallbackContext> action)
   {
      inputAction.performed -= action;
   }
   private void UnsubscribeCanceledAction(InputAction inputAction,Action<InputAction.CallbackContext> action)
   {
      inputAction.canceled -= action;
   }
   public void UnsubscribeActions()
   {
      foreach (var item in _inputActionsContainer.Values)
      {
         foreach (var a in _receiverActionsContainer[item])
         {
            for (int i = 0; i < _statesActionsContainer[a][0]; i++)
            {
               _unsubscribeActionsContainer[i](item,a);
            }
         }
      }
      
      // _movementInputAction.performed -= _movementReceiver;
      // _movementInputAction.Disable();    
      // _scrollInputAction.started -= _changeSkillReceiver;
      // _scrollInputAction.Disable();
      // _throwSkillInputAction.started -= _throwSkillReceiver;
      // _throwSkillInputAction.canceled -= _cancelSkillReceiver;
      // _throwSkillInputAction.Disable();

   }
   
   
}