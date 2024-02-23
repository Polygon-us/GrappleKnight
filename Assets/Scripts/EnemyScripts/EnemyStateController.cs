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

    public bool DoState()
    {
        float velocity = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _rigidbody2D.gravityScale)*-2)*_rigidbody2D.mass;
        _rigidbody2D.AddForce(Vector2.up*velocity, ForceMode2D.Impulse);
        _rigidbody2D.AddForce(Vector2.right*velocity, ForceMode2D.Impulse);
        return false;
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

    private bool _isOnState;
    void FixedUpdate()
    {
        if (_isOnState)
        {
            if (!_currentState.DoState())
            {
                StopStates();
                //_currentState = _enemyStateManager.GetNextState();
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
