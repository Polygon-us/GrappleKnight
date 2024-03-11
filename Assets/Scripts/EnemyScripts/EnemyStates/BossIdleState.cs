using System;
using UnityEngine;

public class BossIdleState : IState
{
    private bool initStates = false;

    private float _waitTime;
    private float _currentTime;
    private EnemyStateEnum _nextEnemyStateEnum;

    private BoxCollider2D _activationZoneCollider;
    public BossIdleState(float waitTime, EnemyStateEnum nextEnemyStateEnum, BoxCollider2D activationZoneCollider)
    {
        _waitTime = waitTime;
        _nextEnemyStateEnum = nextEnemyStateEnum;
        _activationZoneCollider = activationZoneCollider;
    }
    public void StartStates()
    {

    }
    public void StartState()
    {
        if (_activationZoneCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            initStates = true;
        }
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        StartState();
        if (initStates == true)
        {
            if (_currentTime >= _waitTime)
            {
                _currentTime = 0;
                enemyStateEnum = _nextEnemyStateEnum;
                return false;
            }

        }
        _currentTime += Time.deltaTime;
        enemyStateEnum = _nextEnemyStateEnum;
        return true;
    }

    public Action<Collision2D> CollisionAction()
    {
        return null;
    }

    public Action<Collider2D> ColliderAction()
    {
        return null;
    }
}
