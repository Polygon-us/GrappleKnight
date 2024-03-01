using System;
using UnityEngine;

public class BlastWaveAttackState : IState
{
    private float _jumpHeight;
    private float _forceDown = 20f;
    private float _waveDuracion = 0.5f;
    private float _delayBeforeReturn = 0.1f;
    private float _waveImpactForce;

    private Vector3 _inicialWaveScale = new Vector3(1, 0.1f);
    private Vector3 _finalWaveScale = new Vector3(16, 0.1f);
    

    private float _transitionTimer = 0f;
    private float _currentTime;
    private float _timeOfJump;

    private bool _reversed = false;
    private bool _jumping = false;
    private bool _expandWave = false;
    private bool _onFloor = false;



    private Rigidbody2D _myRigidbody;
    private Rigidbody2D _playerRigidbody;
    private CapsuleCollider2D _myCapsuleCollider2D;
    private BoxCollider2D _waveCollider;
    private Transform _wave;
    private Transform _playerTransform;

    public BlastWaveAttackState(float jumpHeight, Rigidbody2D myRigidbody, 
         CapsuleCollider2D capsuleCollider2D, Transform wave, Vector3 finalWaveScale,
         float waveDuracion, Rigidbody2D playerRigidbody, BoxCollider2D waveCollider, 
         float waveImpactForce, Transform playerTransform, float forceDown, float delayBeforeReturn, Vector3 inicialWaveScale)
    {
        _jumpHeight = jumpHeight;
        _myRigidbody = myRigidbody;
        _myCapsuleCollider2D = capsuleCollider2D;
        _wave = wave;
        _finalWaveScale = finalWaveScale;
        _waveDuracion = waveDuracion;
        _playerRigidbody = playerRigidbody;
        _waveCollider = waveCollider;
        _waveImpactForce = waveImpactForce;
        _playerTransform = playerTransform;
        _forceDown = forceDown;
        _delayBeforeReturn = delayBeforeReturn;
        _inicialWaveScale = inicialWaveScale;
    }

    

    bool ExpandWave()
    {
        if (_expandWave)
        {
            if (!_reversed)
            {
                _transitionTimer += Time.deltaTime;

                float progress = Mathf.Clamp01(_transitionTimer / _waveDuracion);

                Vector3 newScale = Vector3.Lerp(_inicialWaveScale, _finalWaveScale, progress);

                _wave.localScale = newScale;

                if (_transitionTimer >= _waveDuracion)
                {
                    _transitionTimer = 0f; 
                    _reversed = true; 
                }
            }
            else
            {
                _transitionTimer += Time.deltaTime;

                if (_transitionTimer >= _delayBeforeReturn)
                {
                    float progress = Mathf.Clamp01((_transitionTimer - _delayBeforeReturn) / _waveDuracion);
                    Vector3 newScale = Vector3.Lerp(_finalWaveScale, _inicialWaveScale, progress);
                    _wave.localScale = newScale;

                    if (_transitionTimer >= _waveDuracion + _delayBeforeReturn)
                    {
                        _transitionTimer = 0f;
                        _reversed = false; 
                            _expandWave = false;
                            _jumping = false;
                            return false;
                    }
                }
            }
        }
        return true;
    }
    
    public void WaveStroke()
    {
        if (_waveCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            if (_wave.position.x > _playerTransform.position.x)
            {
                Debug.Log("me golpeo la onda izquierda");
                _playerRigidbody.AddForce(-Vector2.right * _waveImpactForce);
                _playerTransform.GetComponent<ILife>().ReduceLife(1);


            }

            else if (_wave.position.x < _playerTransform.position.x)
            {
                Debug.Log($"me golpeo la onda: {_playerRigidbody}");

                _playerRigidbody.AddForce(Vector2.right * _waveImpactForce);
                _playerTransform.GetComponent<ILife>().ReduceLife(1);
            }
        }
    }
    void Jump()
    {
        if (!_jumping)
        {
            _jumping = true;
            float velocity = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _myRigidbody.gravityScale) * -2) * _myRigidbody.mass;
            _timeOfJump = -1 * _jumpHeight / (Physics2D.gravity.y * _myRigidbody.gravityScale);
            _myRigidbody.AddForce(velocity * Vector2.up, ForceMode2D.Impulse);
            _currentTime = Time.deltaTime;
        }
        else
        {
            if (_currentTime!=0)
            {
                _currentTime += Time.deltaTime;
                if (_currentTime >= _timeOfJump)
                {
 
                    _currentTime = 0;
                    ApplyDownForce();
                }

            }
          
        }

    }

    void ApplyDownForce()
    {
        _onFloor = true;
        _myRigidbody.AddForce(Vector2.down * _forceDown, ForceMode2D.Impulse);
    }

    public void StartState()
    {
        
    }

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
        if (!ExpandWave())
        {
            enemyStateEnum = EnemyStateEnum.Idle;
            return false;
        }
        Jump();
        WaveStroke();

        enemyStateEnum = EnemyStateEnum.Idle;
        return true;
    }

    private void CollisionEnter(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor") && _onFloor)
        {
            _expandWave = true;
            _onFloor = false;
        }
    }

    public Action<Collision2D> CollisionAction()
    {
        return CollisionEnter;
    }
}

