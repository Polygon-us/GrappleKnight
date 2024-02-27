using System;
using UnityEngine;

public class JumpToPlayerAttackState : IState
{
    private Rigidbody2D _rigidbody2D;
    private float _jumpHeight;


    private Transform _bossTransform;
    private Transform _playerTransform;

    private float _currentTime;
    private float _timeOfJump;

    private bool _isStartState = false;
    public JumpToPlayerAttackState(Rigidbody2D rigidbody2D, float jumpHeight, Transform bossTransform ,Transform playerTransform)
    {
        _rigidbody2D = rigidbody2D;
        _jumpHeight = jumpHeight;
        _bossTransform = bossTransform;
        _playerTransform = playerTransform;

    }

    private void StartState()
    {
        _isStartState = true;
        float direccion = _playerTransform.position.x - _bossTransform.position.x;
        Vector2 _bossPosition = _bossTransform.position;
        Vector2 _playerPosition = _playerTransform.position;
         _jumpHeight = ((_playerPosition.y*(_bossPosition.x*_bossPosition.x))-(_bossPosition.y
            *(_playerPosition.x*_playerPosition.x))) / ((_bossPosition.x*_bossPosition.x)-(_playerPosition.x*_playerPosition.x));
 
        _jumpHeight = Mathf.Abs(_jumpHeight);
        // float a = (_jumpHeight -_bossPosition.y) / (_bossPosition.x * _bossPosition.x);
        // Debug.Log(_jumpHeight);
        // float x = Mathf.Sqrt((_jumpHeight - _bossPosition.y) / a);
        // Debug.Log($"a: {a} ...x: {x}");
        // if (Mathf.Approximately(a,_bossPosition.x))
        // {
        //     x = -Mathf.Sqrt((_jumpHeight - _bossPosition.y) / a);
        //     Debug.Log($"x2: {x}");
        // }
        float velocity = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _rigidbody2D.gravityScale)*-2)*_rigidbody2D.mass;
        _timeOfJump = -2 * velocity / (Physics2D.gravity.y * _rigidbody2D.gravityScale);
        float velocityDos = (Mathf.Abs(direccion) / _timeOfJump) *_rigidbody2D.mass;
  
        //Debug.Log(velocity);

       _rigidbody2D.AddForce(Vector2.up*velocity, ForceMode2D.Impulse);
       _rigidbody2D.AddForce(Vector2.right*Mathf.Sign(direccion)*velocityDos, ForceMode2D.Impulse);

        
    }
    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        if (!_isStartState)
        {
            StartState();
        }
        _currentTime += Time.fixedDeltaTime;
        if (_currentTime>=_timeOfJump)
        {
            _currentTime = 0;
            _isStartState = false;
            enemyStateEnum = EnemyStateEnum.Idle;
            return false;
        }
        enemyStateEnum = EnemyStateEnum.Idle;
        return true;

    }

    public Action<Collision2D> CollisionAction()
    {
        return null;
    }
}