using System;
using UnityEngine;
public class Boss : MonoBehaviour
{
    //[SerializeField] private EnemiesScriptableObjectTemplate configBoss;
    

    //Variables de configuracion del boss

    //salto
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _forceDown = 20f;
    //onda
    [SerializeField] private float _waveDuracion = 0.5f;
    [SerializeField] private float _waveImpactForce = 3f;
    [SerializeField] private float _delayBeforeReturn = 0.1f;
    [SerializeField] private Vector3 _finalWaveScale = new Vector3(16, 0.1f);
    [SerializeField] private Vector3 _inicialWaveScale = new Vector3(1, 0.1f);
    //vida
    [SerializeField] private int _maxLife;


    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _wave;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private BoxCollider2D _waveCollider;
    [SerializeField] private BoxCollider2D activationZoneCollider;

    [SerializeField]private int _chargeMaxNumber;
    [SerializeField]private float _chargeVelocity;
    
    
    private CapsuleCollider2D _capsuleCollider2D;
    private Rigidbody2D _rigidbody2D;
    
    private EnemyLife _enemyLife;
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;
    private CollisionEvents _collisionEvents;




    private void Awake()
    {
        _enemyStateManager = new EnemyStateManager();
        _enemyStateController = GetComponent<EnemyStateController>();
        _collisionEvents = new CollisionEvents();
        _enemyStateController.Configure(_enemyStateManager, _collisionEvents);

        
        _enemyLife = new EnemyLife(gameObject,_maxLife);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        
        
        AddStates();
        InitialState();
        BeginStates();
    }
    private void AddStates()
    {
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.BlastWave, new BlastWaveAttackState(_jumpHeight, _rigidbody2D,
            _capsuleCollider2D, _wave, _finalWaveScale, _waveDuracion, _playerRigidbody, _waveCollider,
            _waveImpactForce, _playerTransform, _forceDown, _delayBeforeReturn, _inicialWaveScale));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.JumpAttack, new JumpToPlayerAttackState(_rigidbody2D,
            _jumpHeight, transform, _playerTransform));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.ChargeImpact, new ChargeImpactAttackState(_rigidbody2D,
            transform, _playerTransform, _chargeMaxNumber, _chargeVelocity));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Idle, new BossIdleState(1,EnemyStateEnum.Random, activationZoneCollider));
    }
    private void InitialState()
    {
        _enemyStateController.ChangeCurrentState(EnemyStateEnum.Idle);
    }
    private void BeginStates()
    {
        _enemyStateController.StartStates();
    }
    private void EndStates()
    {
        _enemyStateController.StopStates();
    }
    
    public void ReduceEnemyLife(int amount)
    {
        if (_enemyLife.ReduceLife(amount))
        {
            EndStates();
            _enemyStateController.UnsubscribeCollisionAction();
            _enemyLife.DeactivateEnemy();
        }
    }

    private void OnDestroy()
    {
        ReduceEnemyLife(_maxLife);
    }
}