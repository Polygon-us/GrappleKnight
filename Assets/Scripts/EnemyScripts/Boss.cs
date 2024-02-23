using System;
using UnityEngine;
public class Boss : MonoBehaviour
{
    private EnemyLife _enemyLife;
    [SerializeField]private int _maxLife;
    [SerializeField] private float _jumpHeight;
    
    private Rigidbody2D _rigidbody2D;
    
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;
    private void Awake()
    {
        _enemyLife = new EnemyLife(gameObject,_maxLife);
        _enemyStateManager = new EnemyStateManager();
        _enemyStateController = GetComponent<EnemyStateController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        AddStates();
        InitialState();
        BeginStates();
    }
    private void AddStates()
    {
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.AttackOne, new JumpOnPlayerAttackState(_rigidbody2D,_jumpHeight));
        //_enemyStateManager.FillStatesContainer(EnemyStateEnum.AttackTwo, new ChargeImpactAttackState());
    }
    private void InitialState()
    {
        _enemyStateController.ChangeCurrentState(_enemyStateManager.GetNextState());
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