using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private InputAction _movementInputAction;

    private IMovable _currentMovement;

    private IMovable _lastMovement;
    private IMovable _queueMovement;

    private bool _isReadyToMove;
    private void FixedUpdate()
    {
        if (_isReadyToMove && _movementInputAction != null)
        {
            _currentMovement.DoMove(_movementInputAction);
        }
    }

    public void ChangeCurrentMovement(IMovable movement)
    {
        _currentMovement = movement;
    }
    public void ChangeCurrentMovement()
    {
        if (_queueMovement!=null)
        {
            _lastMovement = _currentMovement;
            _currentMovement = _queueMovement;
            _queueMovement = null;
        }
        else
        {
            _currentMovement = _lastMovement;
        }
    }

    public void QueueMovement(IMovable movement)
    {
        _queueMovement = movement;
    }

    public void StarMovement()
    {
        _isReadyToMove = true;
    }
    public void InputActionMovement(InputAction.CallbackContext callbackContext)
    {
        _movementInputAction = callbackContext.action;
    }
    
}
