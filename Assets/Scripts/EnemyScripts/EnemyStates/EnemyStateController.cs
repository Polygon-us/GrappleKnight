using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    private IState _currentState;
    private EnemyStateManager _enemyStateManager;
    private void Awake()
    {
        _enemyStateManager = new EnemyStateManager();
        var patrolState = new EnemyPatrolState();
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Patrol, patrolState);
        SetState(EnemyStateEnum.Patrol);
    }
    void FixedUpdate()
    {
        if (_currentState != null)
        {
            _currentState.DoState(); 
        }
    }
    private void SetState(EnemyStateEnum stateEnum)
    {
        _currentState = _enemyStateManager.GetNextState(stateEnum);
    }
}

