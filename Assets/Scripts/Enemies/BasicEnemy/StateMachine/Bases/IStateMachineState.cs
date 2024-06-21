namespace Enemies.BasicEnemy.StateMachine.Bases
{
    public interface IStateMachineState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
