using System;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]private int _maxLife;

    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private BoxCollider2D _persecutorCollider;

    [Header("Damage")] 
    [SerializeField] [Range(1, 100)] private int _percentDamage;

    private bool _isHuntingMode;

    private EnemyLife _enemyLife;
    private EnemyTeleporter _enemyTeleporter;
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;
    private CollisionEvents _collisionEvents;

    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyStateController = GetComponent<EnemyStateController>();
        
        _enemyLife = new EnemyLife(gameObject,_maxLife);
        _enemyStateManager = new EnemyStateManager();
        _collisionEvents = new CollisionEvents();
        _enemyStateController.Configure(_enemyStateManager, _collisionEvents);

        _enemyTeleporter = GetComponent<EnemyTeleporter>();
        _enemyTeleporter.Configure(_pointA);

        AddStates();
        InitialState();
        BeginStates();
        
    }
    private void AddStates()
    {
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Patrol, new EnemyPatrolState(_pointA,_pointB,_rigidbody,
            transform,_collisionEvents));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Hunt, new EnemyHuntState( _persecutorCollider, _rigidbody,
            transform,  playerTransform,  _isHuntingMode,_pointA,_pointB,_collisionEvents,_percentDamage));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Idle, new EnemyIdleState(1,EnemyStateEnum.Patrol));
    }
    private void InitialState()
    {
        _enemyStateController.ChangeCurrentState(EnemyStateEnum.Patrol);

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
            _enemyLife.DeactivateEnemy();
        }
    }
    
}
