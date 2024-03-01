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

    private bool _isAvailableHurtPlayer;

    private bool _isOnState;
    public JumpToPlayerAttackState(Rigidbody2D rigidbody2D, float jumpHeight, Transform bossTransform ,Transform playerTransform)
    {
        _rigidbody2D = rigidbody2D;
        _jumpHeight = jumpHeight;
        _bossTransform = bossTransform;
        _playerTransform = playerTransform;
    }

    public void StartState()
    {
        _bossTransform.gameObject.layer = LayerMask.NameToLayer("Default");
        _currentTime = 0;
        _isOnState = true;
        //_playerTransform.gameObject.layer = LayerMask.NameToLayer("Default");
        float direccion = _playerTransform.position.x - _bossTransform.position.x;
        Vector2 _bossPosition = _bossTransform.position;
        Vector2 _playerPosition = _playerTransform.position;

        float velocity = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _rigidbody2D.gravityScale)*-2)*_rigidbody2D.mass;
        _timeOfJump = -2 * velocity / (Physics2D.gravity.y * _rigidbody2D.gravityScale);
        float velocityDos = (Mathf.Abs(direccion) / _timeOfJump) *_rigidbody2D.mass;
  
        //Debug.Log(velocity);

       _rigidbody2D.AddForce(Vector2.up*velocity, ForceMode2D.Impulse);
       _rigidbody2D.AddForce(Vector2.right*Mathf.Sign(direccion)*velocityDos, ForceMode2D.Impulse);
       _isAvailableHurtPlayer = true;

    }
    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        if (_currentTime >= _timeOfJump/2 && _bossTransform.position.y <= -0.75)
        {
            _bossTransform.gameObject.layer = LayerMask.NameToLayer("Invulnerability");
        }
        _currentTime += Time.fixedDeltaTime;
        if (!_isOnState)
        {
            _currentTime = 0;
            enemyStateEnum = EnemyStateEnum.Idle;
            return false;
        }
        enemyStateEnum = EnemyStateEnum.Idle;
        return true;

    }

    private void CollisionEnter(Collision2D other)
    {
        if (_isAvailableHurtPlayer)
        {
            if (other.transform == _playerTransform )
            {
                //_isAvailableHurtPlayer = false;
                //other.gameObject.layer = LayerMask.NameToLayer("Invulnerability");
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHAHAHAHAHAHHAHHHHHHHHHHHHH");
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Floor") && _currentTime >= _timeOfJump)
            {
                _currentTime = 0;
                
                Collider2D[] colliders = Physics2D.OverlapCircleAll(_bossTransform.transform.position, 2);
                foreach (Collider2D item in colliders)
                {
                    if (item.TryGetComponent<ILife>(out ILife life))
                    {
                        life.ReduceLife(1);
                    }
                }
                _bossTransform.gameObject.layer = LayerMask.NameToLayer("Default");
                _isAvailableHurtPlayer = false;
                _isOnState = false;
            }
        }
    }
    public Action<Collision2D> CollisionAction()
    {
        return CollisionEnter;
    }
}