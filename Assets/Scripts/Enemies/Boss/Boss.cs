using System;
using UnityEngine;
public class Boss : MonoBehaviour
{
    //[SerializeField] private EnemiesScriptableObjectTemplate configBoss;
    

    //Variables de configuracion del boss

    //salto
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _forceDown = 20f;
    [SerializeField] private float _waveDuracion = 0.5f;
    [SerializeField] private float _waveImpactForce = 3f;
    [SerializeField] private float _delayBeforeReturn = 0.1f;
    [SerializeField]private float _chargeVelocity;

    [SerializeField] private int _maxLife;
    [SerializeField]private int _chargeMaxNumber;
    [SerializeField] [Range(1, 100)] private int _percentDamage;
    
    [SerializeField] private Vector3 _inicialWaveScale = new Vector3(1, 0.1f);
    [SerializeField] private Vector3 _finalWaveScale = new Vector3(16, 0.1f);

    private bool _isFigthing = false;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _wave;

    [SerializeField] private Rigidbody2D _playerRigidbody;

    [SerializeField] private BoxCollider2D _waveCollider;
    [SerializeField] private BoxCollider2D activationZoneCollider;

    private CapsuleCollider2D _capsuleCollider2D;
    private Rigidbody2D _rigidbody2D;
    
    private EnemyLife _enemyLife;
    private EnemyStateManager _enemyStateManager;
    private EnemyStateController _enemyStateController;
    private CollisionEvents _collisionEvents;

    private void Awake()
    {
        //RespawnTrigger.OnCollision += Message;
        _enemyStateManager = new EnemyStateManager();
        _enemyStateController = GetComponent<EnemyStateController>();
        _collisionEvents = new CollisionEvents();
        _enemyStateController.Configure(_enemyStateManager, _collisionEvents);
        
        _enemyLife = new EnemyLife(gameObject,_maxLife);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        
        AddStates();
    }
    private void AddStates()
    {
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.BlastWave, new BlastWaveAttackState(_jumpHeight, _rigidbody2D,
            _capsuleCollider2D, _wave, _finalWaveScale, _waveDuracion, _playerRigidbody, _waveCollider,
            _waveImpactForce, _playerTransform, _forceDown, _delayBeforeReturn, _inicialWaveScale,_percentDamage));

        _enemyStateManager.FillStatesContainer(EnemyStateEnum.JumpAttack, new JumpToPlayerAttackState(_rigidbody2D,
            _jumpHeight, transform, _playerTransform, _percentDamage, _collisionEvents));
        
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.ChargeImpact, new ChargeImpactAttackState(_rigidbody2D,
            transform, _playerTransform, _chargeMaxNumber, _chargeVelocity, _percentDamage));
        
        _enemyStateManager.FillStatesContainer(EnemyStateEnum.Idle, new BossIdleState(1,EnemyStateEnum.Random));
    }
    public void InitialState()
    {
        _enemyStateController.ChangeCurrentState(EnemyStateEnum.Idle);
    }
    //private void Message(Vector3 vec)
    //{
    //    Debug.Log("Hola Amiguitos, como estan?");
    //}
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
        //RespawnTrigger.OnCollision -= Message;
    }
}