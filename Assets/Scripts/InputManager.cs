using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
   private PlayerInputAction _playerInputAction;

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
   }

   private void EnableInputAction(PlayerInputTypeEnum playerInputTypeEnum)
   {
      if (!_inputActionsContainer[playerInputTypeEnum].enabled)
      {
         _inputActionsContainer[playerInputTypeEnum].Enable();
      }
   }
   
   public void SubscribeStartedAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,1);
      _inputActionsContainer[playerInputTypeEnum].started += action;
      EnableInputAction(playerInputTypeEnum);
   }
   public void SubscribePerformedAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,2);
      _inputActionsContainer[playerInputTypeEnum].performed += action;
      EnableInputAction(playerInputTypeEnum);
   }
   public void SubscribeCanceledAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,3);
      _inputActionsContainer[playerInputTypeEnum].canceled += action;
      EnableInputAction(playerInputTypeEnum);
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
         item.Disable();
      }
   }
   
   
}