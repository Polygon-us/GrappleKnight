
using System.Collections.Generic;

public class EnemyStateManager
{
    private Dictionary<EnemyStateEnum, IState> _enemyStatesContainer = new Dictionary<EnemyStateEnum, IState>();

    public void FillStatesContainer(EnemyStateEnum enemyStateEnum, IState state)
    {
        _enemyStatesContainer.Add(enemyStateEnum, state);
    }

    public IState GetNextState(EnemyStateEnum stateEnum)
    {
        if (_enemyStatesContainer.TryGetValue(stateEnum, out IState nextState))
        {
            return nextState;
        }
        else
        {
            return null;
        }
    }
}
