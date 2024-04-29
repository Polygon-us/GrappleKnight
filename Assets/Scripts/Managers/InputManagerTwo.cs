using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerTwo : MonoBehaviour
{
    private PlayerInputAction _playerInputAction;

    public static PlayerInputAction.PlayerSkillActionMapActions skillsMap;
    public static PlayerInputAction.PlayerMovementActions movementMap;

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
        SetMaps();
    }
    
    private void SetMaps()
    {
        skillsMap = _playerInputAction.PlayerSkillActionMap;
        movementMap = _playerInputAction.PlayerMovement;
        
       skillsMap.Enable();
       movementMap.Enable();
    }
    
}