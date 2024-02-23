using UnityEditor.IMGUI.Controls;
using UnityEngine;

public interface IState
{
    bool DoState();
}

public class JumpOnPlayerAttackState : IState
{
    public bool DoState()
    {
        throw new System.NotImplementedException();
    }
}

public class ChargeImpactAttackState : IState
{
    public bool DoState()
    {
        throw new System.NotImplementedException();
    }
}
public class EnemyStateController : MonoBehaviour
{
    private IState _currentState;
    private EnemyStateManager _enemyStateManager;
    void FixedUpdate()
    {
        if (_currentState.DoState())
        {
            //_enemyStateManager.
        }
    }
    
    
}
