namespace Enemies.BasicEnemy.StateMachine.Bases
{
    public interface IStateMachine
    {
        State CurrentState { get; set; }
        
        void ChangeState(State newState);
        void ChangeState(State newState, bool enter);

        void StopMachine();
        void PlayMachine();

        void UpdateMachine();
    }
}
