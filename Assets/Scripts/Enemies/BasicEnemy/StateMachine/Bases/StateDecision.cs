using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.Bases
{
    public abstract class StateDecision : ScriptableObject
    {
        public delegate void DecisionDelegate(StateDecision decision);
        public event DecisionDelegate OnConditionChanged;
        
        private bool _condition;

        public bool StateCondition
        {
            get => _condition;
            protected set
            {
                _condition = value;
                OnConditionChanged?.Invoke(this);
            }
        }
        
        public abstract void InitDecision(StateMachine machineReference);
        public abstract void UpdateDecision();
    }
}
