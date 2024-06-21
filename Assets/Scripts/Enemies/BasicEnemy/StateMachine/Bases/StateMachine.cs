using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.Bases
{
    public abstract class StateMachine : MonoBehaviour, IStateMachine
    {
        State IStateMachine.CurrentState { get; set; }
        
        private State CurrentState
        {
            get => ((IStateMachine)this).CurrentState;
            set => ((IStateMachine)this).CurrentState = value;
        }

        protected bool isMachineRunning { get; private set; } = false;

        public void ChangeState(State newState)
        {
            if(newState == null) return;

            if (CurrentState != newState)
            {
                if (CurrentState != null)
                {
                    CurrentState.OnEnterState -= PlayMachine;
                    CurrentState.OnExitState -= StopMachine;
                }
                
                CurrentState = newState;
                
                CurrentState.OnEnterState += PlayMachine;
                CurrentState.OnExitState += StopMachine;
            }
        }

        public void ChangeState(State newState, bool enter)
        {
            if(newState == null) return;
            
            if(enter) CurrentState?.Exit(); 
            
            ChangeState(newState);
            
            if(enter) CurrentState?.Enter();
        }

        public void StopMachine()
        {
            isMachineRunning = false;
        }

        public void PlayMachine()
        {
            isMachineRunning = true;
        }

        public virtual void UpdateMachine()
        {
            if(isMachineRunning) CurrentState?.Update();
        }
    }
}
