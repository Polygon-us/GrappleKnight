using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : IState
{
    private float waitTime = 1f;
    private float currentTime;
    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        Debug.Log($"Idle {currentTime}");
        currentTime += Time.deltaTime;
        if (currentTime>=waitTime)
        {
            currentTime = 0;
            enemyStateEnum = EnemyStateEnum.Patrol;
            return false;
        }
        enemyStateEnum = EnemyStateEnum.Patrol;
        return true;
    }

    
}
