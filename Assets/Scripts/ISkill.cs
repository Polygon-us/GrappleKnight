using UnityEngine;

public interface ISkill
{
    void InitSkill();
    bool DoSkill();
    void UndoSkill();

    PlayerMovementTypeEnum SendActionMapTypeEnum();
    void UnsubscribeActions();

}