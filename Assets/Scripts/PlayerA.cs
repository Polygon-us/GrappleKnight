using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class PlayerA : MonoBehaviour
{
    [SerializeField] private Transform _hookBegin;
    [SerializeField] private Transform _hookEnd;
    [SerializeField] private Transform _Boomerang;
    [SerializeField] private GameObject _rope;
    
    private SpringJoint2D _springJoint2D;

    private const float _hookMaxDistance = 10;
    
    private const float _boomerangMaxDistance = 4;
    private const float _boomerangSpeed = 4f;
    
    private InputManager _inputManager;
    private PlayerInputAction _playerInputAction;
    
    private SkillManager _skillStateManager;

    private PlayerSkillController _playerSkillController;
    
    
    private void Awake()
    {
        AssignModules();
        FillSkillStateManager();
        ChangeSkill();
    }

    private void AssignModules()
    {
        _playerInputAction = new PlayerInputAction();
        _inputManager = new InputManager(_playerInputAction,ChangeSkill,ThrowSkill,CancelSkill);
        _inputManager.Configure();
        _skillStateManager = new SkillManager();
        _playerSkillController = GetComponent<PlayerSkillController>();
        _springJoint2D = GetComponent<SpringJoint2D>();
    }

    private void FillSkillStateManager()
    {
        _skillStateManager.AddSkill(new HookSkill(transform,_hookEnd,_hookBegin,_springJoint2D,_hookMaxDistance,_rope));
        _skillStateManager.AddSkill(new BoomerangSkill(transform,_Boomerang,_boomerangMaxDistance,_boomerangSpeed));
    }


    private void ChangeSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.ChangeCurrentSkill(_skillStateManager.GetNextSkill());
    }
    private void ChangeSkill()
    {
        _playerSkillController.ChangeCurrentSkill(_skillStateManager.GetNextSkill());
    }
    private void ThrowSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.StartSkill();
    }

    private void CancelSkill(InputAction.CallbackContext callbackContext)
    {
        _playerSkillController.StopSkill();
    }
    
    private void OnDestroy()
    {
        _inputManager.UnsuscribeActions();
    }
}