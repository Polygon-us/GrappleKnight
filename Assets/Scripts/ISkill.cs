using UnityEngine;

public interface ISkill
{
    void InitSkill();
    bool DoSkill();
    void UndoSkill();

    PlayerMovementEnum SendActionMapTypeEnum();
    void UnsubscribeActions();

}