using System;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    private IState _currentState;
    private EnemyStateManager _enemyStateManager;
    private CollisionEvents _collisionEvents;
    
    private bool _isOnState;

    private Action<Collision2D> _onCollisionEnter;
    private Action<Collision2D> _onCollisionReceiver;
    
    private Action<Collider2D> _onTriggerEnter;
    private Action<Collider2D> _onTriggerReceiver;
    
    public void Configure(EnemyStateManager enemyStateManager, CollisionEvents collisionEvents)
    {
        _enemyStateManager = enemyStateManager;
        _collisionEvents = collisionEvents;
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
        SubscribeCollisionAction(_currentState.CollisionAction(),_currentState.ColliderAction());
    }

    private void SubscribeCollisionAction(Action<Collision2D> collisionAction, Action<Collider2D> colliderAction)
    {
        if (collisionAction != null)
        {
            _onCollisionReceiver = collisionAction;
            _onCollisionEnter += _onCollisionReceiver;
        }
        if (colliderAction!=null)
        {
            _onTriggerReceiver = colliderAction;
            _onTriggerEnter += _onTriggerReceiver;
        }
    }
    public void UnsubscribeCollisionAction()
    {
        if (_onCollisionReceiver != null)
        {
            _onCollisionEnter -= _onCollisionReceiver;
            _onCollisionReceiver = null;
        }
        if (_onTriggerReceiver != null)
        {
            _onTriggerEnter -= _onTriggerReceiver;
            _onTriggerReceiver = null;
        }
    }
    public void StartStates()
    {
        _isOnState = true;
        _currentState.StartState();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _collisionEvents.CollisionStayDispatch(other);
        if (_onCollisionReceiver != null)
        {
            _onCollisionEnter?.Invoke(other);
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        _collisionEvents.TriggerStayDispatch(other);
        if (_onTriggerEnter != null)
        {
            _onTriggerEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _collisionEvents.TriggerExitDispatch(other);
    }

    
    public void StopStates()
    {
        _isOnState = false;
    }
    
}
