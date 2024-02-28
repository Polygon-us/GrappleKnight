using System;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    private IState _currentState;
    private EnemyStateManager _enemyStateManager;

    private bool _isOnState;

    private Action<Collision2D> _onCollisionEnter;
    private Action<Collision2D> _onCollisionReceiver;
    
    
    public void Configure(EnemyStateManager enemyStateManager)
    {
        _enemyStateManager = enemyStateManager;
    }
    
    void FixedUpdate()
    {
        if (_isOnState)
        {
            if (!_currentState.DoState(out EnemyStateEnum enemyStateEnum))
            {
                ChangeCurrentState(enemyStateEnum);
            }
        }
    }

    public void ChangeCurrentState(EnemyStateEnum state)
    {
        UnsubscribeCollisionAction();
        if (state==EnemyStateEnum.Random)
        {
            _currentState = _enemyStateManager.GetNextState();
            StartStates();
        }
        else
        {
            _currentState = _enemyStateManager.GetNextState(state);
            StartStates();
        }
        SubscribeCollisionAction(_currentState.CollisionAction());
    }

    private void SubscribeCollisionAction(Action<Collision2D> collisionAction)
    {
        if (collisionAction!=null)
        {
            _onCollisionReceiver = collisionAction;
            _onCollisionEnter += _onCollisionReceiver;
        }
    }
    public void UnsubscribeCollisionAction()
    {
        if (_onCollisionReceiver != null)
        {
            _onCollisionEnter -= _onCollisionReceiver;
            _onCollisionReceiver = null;
        }
    }
    public void StartStates()
    {
        _isOnState = true;
        _currentState.StartState();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_onCollisionReceiver != null)
        {
            _onCollisionEnter?.Invoke(other);
        }
    }

    public void StopStates()
    {
        _isOnState = false;
    }
    
    
}
