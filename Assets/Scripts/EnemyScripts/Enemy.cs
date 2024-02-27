using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private int _maxLife;

    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private BoxCollider2D _persecutorCollider;

    private bool _isHuntingMode;

    private EnemyLife _enemyLife;
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Patrol, new EnemyPatrolState(_pointA,_pointB,_rigidbody,transform));
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Hunt, new EnemyHuntState( _persecutorCollider, _rigidbody, transform,  playerTransform,  _isHuntingMode));
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            _isHuntingMode = true;
            if (_isHuntingMode  == true)
            {
                _enemyStateController.ChangeCurrentState(EnemyStateEnum.Hunt);

            }
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isHuntingMode = false;

            if (_isHuntingMode == false)
            {
                _enemyStateController.ChangeCurrentState(EnemyStateEnum.Patrol);

            }
        }
    }
    public void ReduceEnemyLife(int amount)
    {
        if (_enemyLife.ReduceLife(amount))
        {
            _enemyLife.DeactivateEnemy();
        }
    }
    
}
