using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.Bases
{
    public class StateDecision : MonoBehaviour
    {
        public delegate void DecisionDelegate(StateDecision decision);
        public event DecisionDelegate OnConditionChanged;
        
        private bool _condition;

        public bool StateCondition
        {
            get => _condition;
            private set
            {
                _condition = value;
                OnConditionChanged?.Invoke(this);
            }
        }
    }
}
