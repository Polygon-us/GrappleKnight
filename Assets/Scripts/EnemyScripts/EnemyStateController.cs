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
        }
        else
        {
            _currentState = _enemyStateManager.GetNextState(state);
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
        }
    }
    public void StartStates()
    {
        _isOnState = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _onCollisionEnter?.Invoke(other);
    }

    public void StopStates()
    {
        _isOnState = false;
    }
    
    
}
