using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Hook")]
    [SerializeField] private Transform _hookBegin;
    [SerializeField] private Transform _hookEnd;
    [SerializeField] private GameObject _rope;
    [SerializeField] private float _hookMaxDistance = 10;
    [SerializeField] private float _angleOfShut;
    [SerializeField] private Vector2 _swingSpeed = new Vector2(0.01f, 0.1f);
    [SerializeField] private LayerMask _hookMask;
    [SerializeField] private float _shakeForceHook = 5;

    [Header("Boomerang")]
    [SerializeField] private Transform _Boomerang;
    [SerializeField] private float _boomerangMaxDistance = 4;
    [SerializeField] private float _boomerangSpeed = 4f;
    
    
    [Header("Movement")]

    [SerializeField]private float _jumpHeight = 5f;
    [SerializeField]private float _raycastLength = 1.01f;
    [SerializeField]private LayerMask _checkFloorMask;
    [SerializeField, Range(0f, 100f)] public float _maxSpeed = 10f;
    [SerializeField, Range(0f, 1000f)] public float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;
    public Vector2 _moveAxis;

    //[Header("Climbing")]
    //[SerializeField] private float _climbingSpeed;

    [Header("Life")] 
    [SerializeField] private int _maxLife;
    

    private InputManager _inputManager;

    private ILife _playerLife;
    
    private SpringJoint2D _springJoint2D;
    private Rigidbody2D _rigidbody2D;
    
    private SkillManager _skillManager;
    private PlayerSkillController _playerSkillController;

    private PlayerMovementController _playerMovementController;
    private PlayerMovementManager _playerMovementManager;

    private TargetCameraController _targetCameraController;

    private void Awake()
    {
        AssignModules();
        FillSkillManager();
        FillMovementManager();
        //ChangeSkill();
        _targetCameraController = GetComponentInChildren<TargetCameraController>();
        _playerMovementController.ChangeCurrentMovement(
            _playerMovementManager.GetMovable(PlayerMovementEnum.PlayerMovement));
        _playerMovementController.StarMovement();
    }

    private void AssignModules()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerMovementManager = new PlayerMovementManager();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputManager = new InputManager(new PlayerInputAction());
        //_inputManager.Configure();
        _skillManager = new SkillManager();
        _playerSkillController = GetComponent<PlayerSkillController>();
        _springJoint2D = GetComponent<SpringJoint2D>();
        _playerLife = GetComponent<ILife>();
        _playerLife.Configure(_maxLife);
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
         IMovable currentMovable = new HookMover(_rigidbody2D, _springJoint2D, _swingSpeed);
        _playerMovementManager.AddMovable(PlayerMovementEnum.HookMovement,currentMovable);
        _inputManager.SubscribePerformedAction(PlayerInputEnum.Movement,
            currentMovable.GetAction(PlayerInputEnum.Movement));
        
        PlayerMover mover = new PlayerMover(transform, _rigidbody2D, _maxSpeed, _maxAcceleration, _jumpHeight,
            _raycastLength, _checkFloorMask, _maxAirAcceleration, _moveAxis);
        currentMovable = mover;
        mover.OnInputMoveChange += OnPressVertical;

        _playerMovementManager.AddMovable(PlayerMovementEnum.PlayerMovement, currentMovable);
        _inputManager.SubscribePerformedAction(PlayerInputEnum.Movement,
            currentMovable.GetAction(PlayerInputEnum.Movement));
        _inputManager.SubscribePerformedAction(PlayerInputEnum.Jump,
            currentMovable.GetAction(PlayerInputEnum.Jump));

        //currentMovable = new PlayerClimbingMover(_climbingSpeed, transform, _rigidbody2D);
        //_playerMovementManager.AddMovable(PlayerMovementEnum.ClimbingMovement, currentMovable);
        //_inputManager.SubscribePerformedAction(PlayerInputEnum.Movement,
        //    currentMovable.OnGetAction(PlayerInputEnum.Movement));


        //_inputManager.SubscribePerformedAction(PlayerInputEnum.ChangeSkill,ChangeSkill);
        _inputManager.SubscribeStartedAction(PlayerInputEnum.ThrowRightSkill,ThrowRightSkill);
        _inputManager.SubscribeStartedAction(PlayerInputEnum.ThrowLeftSkill,ThrowLeftSkill);
        _inputManager.SubscribeCanceledAction(PlayerInputEnum.ThrowRightSkill,CancelSkill);
        _inputManager.SubscribeCanceledAction(PlayerInputEnum.ThrowLeftSkill,CancelSkill);
    }

    private void OnPressVertical(InputAction action)
    {
        if (action.activeControl.IsPressed())
        {
             //_targetCameraController.SetPosBySide(action.activeControl.name == "d");
            
        }
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
        _inputManager.UnsubscribeActions();
        _skillManager.UnsubscribeActions();
    }
}