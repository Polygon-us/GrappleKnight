using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Hook")]
    [SerializeField] private Transform _hookBegin;
    [SerializeField] private Transform _hookEnd;
    [SerializeField] private GameObject _rope;
    [SerializeField] private float _hookMaxDistance = 6;
    [SerializeField] private float _angleOfShut;
    [SerializeField] private Vector2 _swingSpeed = new Vector2(0.01f, 0.1f);
    [SerializeField] private LayerMask _hookMask;
    [SerializeField] private float _shakeForceHook = 0.7f;

    [Header("Boomerang")]
    [SerializeField] private Transform _Boomerang;
    [SerializeField] private float _boomerangMaxDistance = 8;
    [SerializeField] private float _boomerangSpeed = 2f;
    
    [Header("Movement")]
    [SerializeField]private float _jumpHeight = 3f;
    [SerializeField]private float _raycastLength = 1.01f;
    [SerializeField]private LayerMask _checkFloorMask;
    [SerializeField, Range(0f, 100f)] public float _maxSpeed = 10f;
    [SerializeField, Range(0f, 1000f)] public float _maxAcceleration = 18f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 15f;
    public Vector2 _moveAxis;

    //[Header("Climbing")]
    //[SerializeField] private float _climbingSpeed;

    [Header("Life")]
    [SerializeField] private int _maxLife;

    [Header("Animations")]
    [SerializeField] SpriteRenderer spriteRenderer;

    private ILife _playerLife;
    
    private SpringJoint2D _springJoint2D;
    private Rigidbody2D _rigidbody2D;

    
    private SkillManager _skillManager;
    private PlayerSkillController _playerSkillController;

    private PlayerMovementController _playerMovementController;
    private PlayerMovementManager _playerMovementManager;

    private TargetCameraController2 _targetCameraController;

    private ScopeMover _scopeMover;
    private void Awake()
    {
        _targetCameraController = GetComponentInChildren<TargetCameraController2>();
        AssignModules();
        FillSkillManager();
        FillMovementManager();
        _playerMovementController.ChangeCurrentMovement(
            _playerMovementManager.GetMovable(PlayerMovementEnum.PlayerMovement));
        _playerMovementController.FirstLastMovement();
        _playerMovementController.InitMovement();

        GameManager.OnGamePause += StopPlayer;
    }

    private void StopPlayer(bool stop)
    {
        print($"Paused: {stop}");
        
        _playerMovementController.MovementEnable(!stop);
    }
    
    private void AssignModules()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerMovementManager = new PlayerMovementManager();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _skillManager = new SkillManager();
        _playerSkillController = GetComponent<PlayerSkillController>();
        _springJoint2D = GetComponent<SpringJoint2D>();
        _playerLife = GetComponent<ILife>();
        _playerLife.Configure(_maxLife);
        _scopeMover = GetComponentInChildren<ScopeMover>();
        _scopeMover.Configure(_hookBegin,_hookMaxDistance);
    }

    private void FillSkillManager()
    {
        _skillManager.AddLeftSkill(new HookSkill(transform,_hookEnd,_hookBegin,_springJoint2D,
            _hookMaxDistance,_angleOfShut,_rope,_hookMask, _shakeForceHook));
        _skillManager.AddRightSkill(new BoomerangSkill(transform,_Boomerang,
            _boomerangMaxDistance,_boomerangSpeed));
    }
     private void FillMovementManager()
     {
         IMovable currentMovable = new HookMover(_rigidbody2D, _springJoint2D, _swingSpeed,_hookMaxDistance,_targetCameraController);
        _playerMovementManager.AddMovable(PlayerMovementEnum.HookMovement,currentMovable);
        
        InputManagerTwo.movementMap.Movement.performed += currentMovable.GetAction(PlayerInputEnum.Movement);
        
        PlayerMover mover = new PlayerMover(transform, _rigidbody2D, _maxSpeed, _maxAcceleration, _jumpHeight,
            _raycastLength, _checkFloorMask, _maxAirAcceleration, _moveAxis, _targetCameraController, spriteRenderer);
        currentMovable = mover;
        
        _playerMovementManager.AddMovable(PlayerMovementEnum.PlayerMovement, currentMovable);
        
        InputManagerTwo.skillsMap.ThorwRightSkill.started += ThrowRightSkill;
        InputManagerTwo.skillsMap.ThrowLeftSkill.started += ThrowLeftSkill;
        InputManagerTwo.skillsMap.ThorwRightSkill.canceled += CancelSkill;
        InputManagerTwo.skillsMap.ThrowLeftSkill.canceled += CancelSkill;
    }
     
    
    private void ChangeSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.ChangeCurrentSkill(_skillManager.GetNextLeftSkill(
            out PlayerMovementEnum playerMovementTypeEnum));
        _playerMovementController.QueueMovement(
            _playerMovementManager.GetMovable(playerMovementTypeEnum));
        
    }
    private void ChangeSkill(bool _isLeft)
    {
        if (_isLeft)
        {
            _playerSkillController.ChangeCurrentSkill(_skillManager.GetNextLeftSkill(
                out PlayerMovementEnum playerMovementTypeEnum)); 
            _playerMovementController.QueueMovement(
                _playerMovementManager.GetMovable(playerMovementTypeEnum));
        }
        else
        {
            _playerSkillController.ChangeCurrentSkill(_skillManager.GetNextRightSkill(
                out PlayerMovementEnum playerMovementTypeEnum));
            _playerMovementController.QueueMovement(
                _playerMovementManager.GetMovable(playerMovementTypeEnum)); 
        }
    }
    private void ThrowLeftSkill(InputAction.CallbackContext callbackContext)
    {
        ChangeSkill(true);
        _playerSkillController.StartSkill();
        _playerMovementController.ChangeCurrentMovement(true);
    } 
    private void ThrowRightSkill(InputAction.CallbackContext callbackContext)
    {
        ChangeSkill(false);
        _playerSkillController.StartSkill();
        _playerMovementController.ChangeCurrentMovement(true);
    }

    private void CancelSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.StopSkill();
        _playerMovementController.ChangeCurrentMovement(false);
    }
    
    private void OnDestroy()
    {
        _skillManager.UnsubscribeActions();
        GameManager.OnGamePause -= StopPlayer;
    }
}