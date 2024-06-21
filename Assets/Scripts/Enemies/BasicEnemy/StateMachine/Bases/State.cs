namespace Enemies.BasicEnemy.StateMachine.Bases
{
    public abstract class State : IStateMachineState
    {
        public delegate void StateDelegate();
        
        public event StateDelegate OnEnterState;
        public event StateDelegate OnExitState;
        
        public virtual void Enter()
        {
            OnEnterState?.Invoke();
        }

        public abstract void Update();

        public virtual void Exit()
        {
            OnExitState?.Invoke();
        }
    }
}
