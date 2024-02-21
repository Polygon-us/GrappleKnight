using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerA : MonoBehaviour
{
    [SerializeField] private Transform _hookBegin;
    [SerializeField] private Transform _hookEnd;
    [SerializeField] private Transform _Boomerang;
    [SerializeField] private GameObject _rope;
    
    private SpringJoint2D _springJoint2D;
    private Rigidbody2D _rigidbody2D;

    [SerializeField]private float _hookMaxDistance = 10;
    [SerializeField]private float _angleOfShut;
    [SerializeField]private float _boomerangMaxDistance = 4;
    [SerializeField]private float _boomerangSpeed = 4f;
    [SerializeField]private Vector2 _swingSpeed = new Vector2(0.01f, 0.1f);
    [SerializeField]private LayerMask _hookMask;
    
    private InputManager _inputManager;
    
    private SkillManager _skillManager;
    private PlayerSkillController _playerSkillController;

    private PlayerMovementController _playerMovementController;
    private PlayerMovementManager _playerMovementManager;
    
    private void Awake()
    {
        AssignModules();
        FillSkillManager();
        FillMovementManager();
        ChangeSkill();
        _playerMovementController.ChangeCurrentMovement(
            _playerMovementManager.GetMovable(PlayerMovementTypeEnum.PlayerMovement));
        _playerMovementController.StarMovement();
    }

    private void AssignModules()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerMovementManager = new PlayerMovementManager();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputManager = new InputManager(new PlayerInputAction());
        _inputManager.Configure();
        _skillManager = new SkillManager();
        _playerSkillController = GetComponent<PlayerSkillController>();
        _springJoint2D = GetComponent<SpringJoint2D>();
    }

    private void FillSkillManager()
    {
        _skillManager.AddSkill(new HookSkill(transform,_hookEnd,_hookBegin,_springJoint2D,
            _hookMaxDistance,_angleOfShut,_rope,_hookMask));
        _skillManager.AddSkill(new BoomerangSkill(transform,_Boomerang,
            _boomerangMaxDistance,_boomerangSpeed));
    }
     private void FillMovementManager()
     {
         IMovable currentMovable = new HookMover(_rigidbody2D, _springJoint2D, _swingSpeed);
        _playerMovementManager.AddMovable(PlayerMovementTypeEnum.HookMovement,currentMovable);
        _inputManager.SubscribePerformedAction(PlayerInputTypeEnum.Movement,
            currentMovable.GetAction(PlayerInputTypeEnum.Movement));
        
        currentMovable = new PlayerMover(transform,2);
        _playerMovementManager.AddMovable(PlayerMovementTypeEnum.PlayerMovement, currentMovable);
        _inputManager.SubscribePerformedAction(PlayerInputTypeEnum.Movement,
            currentMovable.GetAction(PlayerInputTypeEnum.Movement));
        
        _inputManager.SubscribePerformedAction(PlayerInputTypeEnum.ChangeSkill,ChangeSkill);
        _inputManager.SubscribeStartedAction(PlayerInputTypeEnum.ThrowSkill,ThrowSkill);
        _inputManager.SubscribeCanceledAction(PlayerInputTypeEnum.ThrowSkill,CancelSkill);
    }
    
    private void ChangeSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.ChangeCurrentSkill(_skillManager.GetNextSkill(
            out PlayerMovementTypeEnum playerMovementTypeEnum));
        _playerMovementController.QueueMovement(
            _playerMovementManager.GetMovable(playerMovementTypeEnum));
        
    }
    private void ChangeSkill()
    {
        _playerSkillController.ChangeCurrentSkill(_skillManager.GetNextSkill(
            out PlayerMovementTypeEnum playerMovementTypeEnum));
        _playerMovementController.QueueMovement(
            _playerMovementManager.GetMovable(playerMovementTypeEnum));
    }
    private void ThrowSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.StartSkill();
        _playerMovementController.ChangeCurrentMovement();
    }

    private void CancelSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.StopSkill();
        _playerMovementController.ChangeCurrentMovement();
    }
    
    private void OnDestroy()
    {
        _inputManager.UnsubscribeActions();
        _skillManager.UnsubscribeActions();
    }
}