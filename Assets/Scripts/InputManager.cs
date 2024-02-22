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
   private Dictionary<Action<InputAction.CallbackContext>,Vector3> _actionStatusContainer =
       new Dictionary<Action<InputAction.CallbackContext>,Vector3>();

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
      _inputActionsContainer.Add(PlayerInputTypeEnum.Jump,_playerInputAction.PlayerMovement.Jump);
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
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,Vector3.right);
      _inputActionsContainer[playerInputTypeEnum].started += action;
      EnableInputAction(playerInputTypeEnum);
   }
   public void SubscribePerformedAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,Vector3.up);
      _inputActionsContainer[playerInputTypeEnum].performed += action;
      EnableInputAction(playerInputTypeEnum);
   }
   public void SubscribeCanceledAction(PlayerInputTypeEnum playerInputTypeEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputTypeEnum], action,Vector3.forward);
      _inputActionsContainer[playerInputTypeEnum].canceled += action;
      EnableInputAction(playerInputTypeEnum);
   }
   
   private void FillContainers(InputAction currentInputAction, Action<InputAction.CallbackContext> action, Vector3 status)
   {
      if (_receiverActionsContainer.TryAdd(currentInputAction,new List<Action<InputAction.CallbackContext>>()))
      {
         _receiverActionsContainer[currentInputAction].Add(action);
      }
      else
      {
         _receiverActionsContainer[currentInputAction].Add(action);
      }
      if (_actionStatusContainer.ContainsKey(action))
      {
         _actionStatusContainer[action] += status;
      }
      else
      {
         _actionStatusContainer.Add(action,status);
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
      foreach (InputAction item in _inputActionsContainer.Values)
      {
         foreach (Action<InputAction.CallbackContext>  jtem in _receiverActionsContainer[item])
         {
            if (_actionStatusContainer[jtem].x==1)
            {
               UnsubscribeStartedAction(item,jtem);
            } 
            if (_actionStatusContainer[jtem].y==1)
            {
               UnsubscribePerformedAction(item,jtem);
            } 
            if (_actionStatusContainer[jtem].z==1)
            {
               UnsubscribeCanceledAction(item,jtem);
            }
         }
         item.Disable();
      }
   }
   
   
}