using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlastWaveAttackState : IState
{
    private float _jumpHeight;
    private float _jumpForce;
    private float _timeOfJump;
    private float _currentTime;
    private float _forceDown = 20f;
    private float _groundCheckDistance = 6.5f;
    private float _shockForceValue = 5f;

    private bool _appliedDownwardForce = false;
    private bool _jumping = false;
    private bool _expandWave = false;
    private bool _onFloor = false;

    private Vector3 _inicialWaveScale = new Vector3(1, 0.1f);
    private Vector3 _finalWaveScale = new Vector3(16, 0.1f);

    private LayerMask _groundLayer;

    private Rigidbody2D _myRigidbody;
    private CapsuleCollider2D _myCapsuleCollider2D;
    private BoxCollider2D _waveCollider;
    private Transform _wave;

    public BlastWaveAttackState(float jumpHeight, Rigidbody2D myRigidbody, LayerMask groundLayer, CapsuleCollider2D capsuleCollider2D)
    {
        _jumpHeight = jumpHeight;
        _myRigidbody = myRigidbody;
        _groundLayer = groundLayer;
        _myCapsuleCollider2D = capsuleCollider2D;
    }

    bool ExpandWave()
    {
        if (_expandWave)
        {

            _expandWave = false;

            return false;
        }
        return true;
    }

    void Jump()
    {
        if (!_jumping)
        {
            _jumping = true;
            _jumpForce = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _myRigidbody.gravityScale) * -2) * _myRigidbody.mass;
            _timeOfJump = -2 * _jumpForce / (Physics2D.gravity.y * _myRigidbody.gravityScale);
            _myRigidbody.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
            Debug.Log($"time of jump : {_timeOfJump}");
        }
        else
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timeOfJump)
            {
                _currentTime = 0;
            }
        }

    }

    void ApplyDownForce()
    {
        RaycastHit2D hit = Physics2D.Raycast(_myRigidbody.position, Vector2.down, _groundCheckDistance, _groundLayer);
        Debug.DrawRay(_myRigidbody.position, Vector2.down * _groundCheckDistance, Color.green);

        if (!hit.collider)
        {
            Debug.Log("fuerza");
            _myRigidbody.AddForce(Vector2.down * _forceDown, ForceMode2D.Impulse);
            _appliedDownwardForce = true;
            _onFloor = true;


            if (_onFloor == true && _myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Floor")))
            {
                Debug.Log("golpe onda");
                _expandWave = true;
            }
        }
        else
        {
            _onFloor = false;
        }
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        Jump();

        ApplyDownForce();


        if (!ExpandWave())
        {
            enemyStateEnum = EnemyStateEnum.BlastWave;
            return false;
        }
        enemyStateEnum = EnemyStateEnum.BlastWave;
        return true;
    }




    public Action<Collision2D> CollisionAction()
    {
        throw new NotImplementedException();
    }
}

