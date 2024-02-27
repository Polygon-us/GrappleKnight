using System;
using UnityEngine;
public class Boss : MonoBehaviour
{
    private EnemyLife _enemyLife;
    [SerializeField]private int _maxLife;
    [SerializeField] private float _jumpHeight;

    [SerializeField] private Transform _playerTransform;
    
    
    private Rigidbody2D _rigidbody2D;
    
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;
    
    private void Awake()
    {
        _enemyLife = new EnemyLife(gameObject,_maxLife);
        _enemyStateManager = new EnemyStateManager();
        _enemyStateController = GetComponent<EnemyStateController>();
        _enemyStateController.Configure(_enemyStateManager);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        AddStates();
        InitialState();
        BeginStates();
    }
    private void AddStates()
    {
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.JumpAttack, new JumpToPlayerAttackState(_rigidbody2D,
            _jumpHeight,transform,_playerTransform));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.ChargeImpact, new ChargeImpactAttackState(_rigidbody2D,
            transform, _playerTransform));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Idle, new EnemyIdleState(1,EnemyStateEnum.Random));
        
        
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