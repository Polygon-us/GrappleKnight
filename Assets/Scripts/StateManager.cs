using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class StateManager<T>
{
    private Dictionary<InputAction,T> _stateContainer = new Dictionary<InputAction, T>();

    public void AddState(InputAction stateType, T state)
    {
        _stateContainer.Add(stateType,state);
    }

    public T GetState(InputAction stateType)
    {
        return _stateContainer[stateType];
    }
    
}