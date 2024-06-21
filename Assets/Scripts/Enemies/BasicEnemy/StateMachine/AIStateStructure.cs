using System;
using System.Collections.Generic;
using System.Linq;
using Enemies.BasicEnemy.StateMachine.Bases;

namespace Enemies.BasicEnemy.StateMachine
{
    [Serializable]
    public class AIState : State
    {
        public string stateName;
        public List<StateAction> actions = new List<StateAction>();
        public List<Transition> transitions = new List<Transition>();

        private AIBrain _aiBrain;

        public void Init(AIBrain aiBrain)
        {
            _aiBrain = aiBrain;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            foreach (Transition element in transitions)
            {
                element.decision.OnConditionChanged += CheckDecision;
            }
        }

        public override void Update()
        {
            foreach (StateAction element in actions)
            {
                element.UpdateAction();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            foreach (Transition element in transitions)
            {
                element.decision.OnConditionChanged -= CheckDecision;
            }
        }

        private void CheckDecision(StateDecision referenceDecision)
        {
            Transition targetTransition = transitions.FirstOrDefault(a => a.decision == referenceDecision);
            
            if (referenceDecision.StateCondition && !string.IsNullOrEmpty(targetTransition.trueState))
            {
                _aiBrain._OnChangeStateRequired?.Invoke(targetTransition.trueState);
            }
            else if (!referenceDecision.StateCondition && !string.IsNullOrEmpty(targetTransition.falseState))
            {
                _aiBrain._OnChangeStateRequired?.Invoke(targetTransition.falseState);
            }
        }
    }

    [Serializable]
    public class Transition
    {
        public StateDecision decision;
        public string trueState;
        public string falseState;
    }
}
