using System;
using UnityEngine;


public class ChargeImpactAttackState : IState
{
    private Rigidbody2D _rigidbody2D;

    private Transform _bossTransform;
    private Transform _playerTransform;

    private int _chargeMaxNumber = 2;
    private int _chargeCount;
    private float _directionSing = 1;

    private bool _isOnState = true;
    

    public ChargeImpactAttackState(Rigidbody2D rigidbody2D,Transform bossTransform ,Transform playerTransform)
    {
        _rigidbody2D = rigidbody2D;
        _bossTransform = bossTransform;
        _playerTransform = playerTransform;

    }
    
    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        //_rigidbody2D.AddForce();
        //_onCollisionEnterAction += CollisionEnter;
        float direccion = _playerTransform.position.x - _bossTransform.position.x;
        //_directionSing = Mathf.Sign(direccion);
        
        Debug.Log(_directionSing);
        _rigidbody2D.velocity = new Vector2(10*_directionSing, 0);
        if (!_isOnState)
        {
            _isOnState = true;
            enemyStateEnum = EnemyStateEnum.Idle;
            return false;
            
        }
        enemyStateEnum = EnemyStateEnum.Idle;
        return true;
    }

    public Action<Collision2D> CollisionAction()
    {
        return CollisionEnter;
    }


    private void CollisionEnter(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && _isOnState)
        {
            Debug.Log("OncollisionEnter");
            _directionSing *= -1;
            _chargeCount += 1;
            if (_chargeCount>_chargeMaxNumber)
            {
                Debug.Log("OncollisionEnterFinished");
                _chargeCount = 0;
                _isOnState = false;
            }
        }
        
    }
}