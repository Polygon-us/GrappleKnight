//using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
//using UnityEngine;
using System.Collections;
using UnityEngine;

public class EnemyStateManager
{
    private Dictionary<EnemyStateEnum,IState> _enemyStatesContainer = new();

    public void FillStatesContainer(EnemyStateEnum enemyStateEnum, IState state)
    {
        _enemyStatesContainer.Add(enemyStateEnum,state);
    }

    public IState GetNextState()
    {
        List<EnemyStateEnum> enumValues = new List<EnemyStateEnum>(_enemyStatesContainer.Keys);
        int randomEnemyStateEnum = Random.Range(0, enumValues.Count);
        EnemyStateEnum enemyStateEnum = enumValues[randomEnemyStateEnum];
        IState enemyState = _enemyStatesContainer[enemyStateEnum];
        return enemyState;
    }

    public IState GetNextState(EnemyStateEnum enemyStateEnum)
    {

        Debug.Log("NextState");
        return _enemyStatesContainer[enemyStateEnum];
    }
}
