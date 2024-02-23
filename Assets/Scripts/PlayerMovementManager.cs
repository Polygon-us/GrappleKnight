using System.Collections.Generic;

public class PlayerMovementManager
{

    private Dictionary<PlayerMovementEnum, IMovable> _MovableContainer =
        new Dictionary<PlayerMovementEnum, IMovable>();
    
    public void AddMovable(PlayerMovementEnum playerMovementEnum,IMovable movable)
    {
        _MovableContainer.Add(playerMovementEnum,movable);
    }

    public IMovable GetMovable(PlayerMovementEnum playerMovementEnum)
    {
        if (playerMovementEnum!=PlayerMovementEnum.None)
        {
            IMovable currentMovable = _MovableContainer[playerMovementEnum];
            return currentMovable;
        }

        return null;
    }
}