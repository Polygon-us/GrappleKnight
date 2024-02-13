using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerA : MonoBehaviour
{
    private InputManager _inputManager;
    private PlayerInputAction _playerInputAction;
    
    private SkillManager _skillStateManager;

    private PlayerSkillController _playerSkillController;
    
    private void Awake()
    {
        AssignModules();
        FillSkillStateManager();
    }

    private void AssignModules()
    {
        _playerInputAction = new PlayerInputAction();
        _inputManager = new InputManager(_playerInputAction,ChangeSkill);
        _inputManager.Configure();
        _skillStateManager = new SkillManager();
        _playerSkillController = new PlayerSkillController();
    }

    private void FillSkillStateManager()
    {
        _skillStateManager.AddSkill(new HookState());
        _skillStateManager.AddSkill(new BoomerangState());
    }


    private void ChangeSkill(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Entree");
        _playerSkillController.ChangeCurrentSkill(_skillStateManager.GetNextSkill());
    }

    private void OnDestroy()
    {
        _inputManager.UnsuscribeActions();
    }
}