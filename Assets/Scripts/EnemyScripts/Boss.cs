using System;
using UnityEngine;
public class Boss : MonoBehaviour
{
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpHeightForBlast;
    
    [SerializeField]private int _maxLife;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private CapsuleCollider2D capsuleCollider2D;

    private Rigidbody2D _rigidbody2D;

    private EnemyLife _enemyLife;
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _enemyStateController = GetComponent<EnemyStateController>();
        
        _enemyLife = new EnemyLife(gameObject,_maxLife);
        _enemyStateManager = new EnemyStateManager();

        _enemyStateController.Configure(_enemyStateManager);

        AddStates();
        InitialState();
        BeginStates();
    }
    private void AddStates()
    {
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.BlastWave, new BlastWaveAttackState(_jumpHeightForBlast,_rigidbody2D, groundLayer,capsuleCollider2D));
        //_enemyStateManager.FillStatesContainer(EnemyStateEnum.AttackTwo, new ChargeImpactAttackState());
    }
    private void InitialState()
    {
        _enemyStateController.ChangeCurrentState(_enemyStateManager.GetNextState(EnemyStateEnum.BlastWave));
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
            _enemyLife.DeactivateEnemy();
        }
    }
}