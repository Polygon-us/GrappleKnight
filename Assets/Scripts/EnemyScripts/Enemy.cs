using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyLife _enemyLife;
    [SerializeField]private int _maxLife;

    private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
 
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;
    private void Awake()
    {
        _enemyLife = new EnemyLife(gameObject,_maxLife);
        _enemyStateManager = new EnemyStateManager();
        _enemyStateController = GetComponent<EnemyStateController>();
        _enemyStateController.Configure(_enemyStateManager);
        _rigidbody = GetComponent<Rigidbody2D>();
        AddStates();
        InitialState();
        BeginStates();
    }
    private void AddStates()
    {
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Patrol, new EnemyPatrolState(_pointA,_pointB,_rigidbody,transform));
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
