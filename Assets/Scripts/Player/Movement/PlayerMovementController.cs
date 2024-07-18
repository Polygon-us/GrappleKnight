using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private IMovable _currentMovement;

    private IMovable _lastMovement;
    private IMovable _queueMovement;

    private bool _isReadyToMove;

    private void FixedUpdate()
    {
        if (_isReadyToMove)
        {
           _currentMovement.DoMove();
        }
    }

    public void FirstLastMovement()
    {
        _lastMovement = _currentMovement;
    }

    public void ChangeCurrentMovement(IMovable movement)
    {
        _currentMovement = movement;
    }

    public void ChangeCurrentMovement(bool isSkillOn)
    {
        if (_queueMovement!=null)
        {
            if (isSkillOn)
            {
                _lastMovement = _currentMovement;
                _currentMovement = _queueMovement;
            }
            else
            {
                _currentMovement = _lastMovement;
            }
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

    public void InitMovement()
    {
        _isReadyToMove = true;
    }
    
    public void MovementEnable(bool canMove = true)
    {
        _currentMovement.EnableMovement(canMove);
    }
}
