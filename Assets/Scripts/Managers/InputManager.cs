using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
   private PlayerInputAction _playerInputAction;

   private Dictionary<PlayerInputEnum, InputAction> _inputActionsContainer =
      new Dictionary<PlayerInputEnum, InputAction>();
   private Dictionary<InputAction, List<Action<InputAction.CallbackContext>>> _receiverActionsContainer =
       new Dictionary<InputAction, List<Action<InputAction.CallbackContext>>>();
   private Dictionary<Action<InputAction.CallbackContext>,Vector3> _actionStatusContainer =
       new Dictionary<Action<InputAction.CallbackContext>,Vector3>();

   // private Action<InputAction, Action<InputAction.CallbackContext>>[] _unsubscribeActionsContainer =
   //    new Action<InputAction, Action<InputAction.CallbackContext>>[3]; 

   public InputManager(PlayerInputAction playerInputAction)
   {
      _playerInputAction = playerInputAction;
      FillInputActionsContainer();
   }

   private void FillInputActionsContainer()
   {
      //_inputActionsContainer.Add(PlayerInputEnum.ChangeSkill,_playerInputAction.PlayerSkillActionMap.ChangeSkill);
      _inputActionsContainer.Add(PlayerInputEnum.ThrowRightSkill,_playerInputAction.PlayerSkillActionMap.ThorwRightSkill);
      _inputActionsContainer.Add(PlayerInputEnum.ThrowLeftSkill,_playerInputAction.PlayerSkillActionMap.ThrowLeftSkill);
      _inputActionsContainer.Add(PlayerInputEnum.Movement,_playerInputAction.PlayerMovement.Movement);
      _inputActionsContainer.Add(PlayerInputEnum.Jump,_playerInputAction.PlayerMovement.Jump);
      
   }


   private void EnableInputAction(PlayerInputEnum playerInputEnum)
   {
      if (!_inputActionsContainer[playerInputEnum].enabled)
      {
         _inputActionsContainer[playerInputEnum].Enable();
      }
   }
   
   public void SubscribeStartedAction(PlayerInputEnum playerInputEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputEnum], action,Vector3.right);
      _inputActionsContainer[playerInputEnum].started += action;
      EnableInputAction(playerInputEnum);
   }
   public void SubscribePerformedAction(PlayerInputEnum playerInputEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputEnum], action,Vector3.up);
      _inputActionsContainer[playerInputEnum].performed += action;
      EnableInputAction(playerInputEnum);
   }
   public void SubscribeCanceledAction(PlayerInputEnum playerInputEnum,Action<InputAction.CallbackContext> action)
   {
      FillContainers(_inputActionsContainer[playerInputEnum], action,Vector3.forward);
      _inputActionsContainer[playerInputEnum].canceled += action;
      EnableInputAction(playerInputEnum);
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