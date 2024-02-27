using Unity.VisualScripting;
using UnityEngine;

public class JumpOnPlayerAttackState : IState
{
    private Rigidbody2D _rigidbody2D;

    private float _jumpHeight;

    public JumpOnPlayerAttackState(Rigidbody2D rigidbody2D, float jumpHeight)
    {
        _rigidbody2D = rigidbody2D;
        _jumpHeight = jumpHeight;
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        //float velocity = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _rigidbody2D.gravityScale)*-2)*_rigidbody2D.mass;
        //_rigidbody2D.AddForce(Vector2.up*velocity, ForceMode2D.Impulse);
        //_rigidbody2D.AddForce(Vector2.right*velocity, ForceMode2D.Impulse);

        //return false;
        throw new System.NotImplementedException();
    }
}

public class ChargeImpactAttackState : IState
{
    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        throw new System.NotImplementedException();
    }
}
public class Saltar : ISkill
{
    public bool DoSkill()
    {
        throw new System.NotImplementedException();
    }

    public void InitSkill()
    {
        throw new System.NotImplementedException();
    }

    public PlayerMovementEnum SendActionMapTypeEnum()
    {
        throw new System.NotImplementedException();
    }

    public void UndoSkill()
    {
        throw new System.NotImplementedException();
    }

    public void UnsubscribeActions()
    {
        throw new System.NotImplementedException();
    }
}
public class EnemyStateController : MonoBehaviour
{
    private IState _currentState;
    private EnemyStateManager _enemyStateManager;

    private bool _isOnState;

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
                //StopStates();
                
                _currentState = _enemyStateManager.GetNextState(enemyStateEnum);
            }
        }
    }

    public void ChangeCurrentState(IState state)
    {
        _currentState = state;
    }
    
    public void StartStates()
    {
        _isOnState = true;
    }

    public void StopStates()
    {
        _isOnState = false;
    }
}
