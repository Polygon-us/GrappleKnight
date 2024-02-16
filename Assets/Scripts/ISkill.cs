public interface ISkill
{
    void InitSkill();
    bool DoSkill();
    void UndoSkill();

    void UnsubscribeActions();
}