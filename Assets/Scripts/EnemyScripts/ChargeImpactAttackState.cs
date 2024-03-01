using System;
using UnityEngine;


public class ChargeImpactAttackState : IState
{
    private Rigidbody2D _rigidbody2D;

    private Transform _bossTransform;
    private Transform _playerTransform;

    private int _chargeMaxNumber;
    private float _chargeVelocity;
    private int _chargeCount;
    private float _directionSing = 1;

    private bool _isOnState = true;

    private Transform _lastCollision2D;
    public ChargeImpactAttackState(Rigidbody2D rigidbody2D,Transform bossTransform ,Transform playerTransform,int chargeMaxNumber, float chargeVelocity)
    {
        _rigidbody2D = rigidbody2D;
        _bossTransform = bossTransform;
        _playerTransform = playerTransform;
        _chargeMaxNumber = chargeMaxNumber;
        _chargeVelocity = chargeVelocity;

    }

    public void StartState()
    {
        _directionSing = Mathf.Sign(_playerTransform.position.x - _bossTransform.position.x);
        _lastCollision2D = null;
        _chargeCount = 0;
        _isOnState = true;
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        if (!_isOnState)
        {
            enemyStateEnum = EnemyStateEnum.Idle;
            return false;
        }
        _rigidbody2D.velocity = new Vector2(_chargeVelocity*_directionSing, 0);
        enemyStateEnum = EnemyStateEnum.Idle;
        return true;
    }

    public Action<Collision2D> CollisionAction()
    {
        return CollisionEnter;
    }
    
    private void CollisionEnter(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && other.transform != _lastCollision2D)
        {
            _lastCollision2D = other.transform;
            _rigidbody2D.velocity = new Vector2(0, 0);
            if (other.transform.TryGetComponent<ILife>(out ILife life))
            {
                float velocity = (1f / 1f) *_playerTransform.GetComponent<Rigidbody2D>().mass;
                _playerTransform.GetComponent<Rigidbody2D>().AddForce(Vector2.right*_directionSing*velocity,ForceMode2D.Impulse);
                life.ReduceLife(1);
            }
            _directionSing = Mathf.Sign(_bossTransform.position.x - other.transform.position.x);
            _chargeCount += 1;
            if (_chargeCount>=_chargeMaxNumber)
            {
                _isOnState = false;
            }
        }
    }
    
}