using System.Collections.Generic;

public class PlayerMovementManager
{

    private Dictionary<PlayerMovementTypeEnum, IMovable> _MovableContainer =
        new Dictionary<PlayerMovementTypeEnum, IMovable>();
    
    public void AddMovable(PlayerMovementTypeEnum playerMovementTypeEnum,IMovable movable)
    {
        _MovableContainer.Add(playerMovementTypeEnum,movable);
    }

    public IMovable GetMovable(PlayerMovementTypeEnum playerMovementTypeEnum)
    {
        IMovable currentMovable = _MovableContainer[playerMovementTypeEnum];
        return currentMovable;
    }
}