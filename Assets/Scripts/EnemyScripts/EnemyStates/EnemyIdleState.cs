using System;
using UnityEngine;

public class EnemyIdleState : IState
{
    private float _waitTime;
    private float _currentTime;
    private EnemyStateEnum _nextEnemyStateEnum;

    public EnemyIdleState(float waitTime, EnemyStateEnum nextEnemyStateEnum)
    {
        _waitTime = waitTime;
        _nextEnemyStateEnum = nextEnemyStateEnum;
    }

    public void StartState()
    {
        
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime>=_waitTime)
        {
            _currentTime = 0;
            enemyStateEnum = _nextEnemyStateEnum;
            return false;
        }
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
