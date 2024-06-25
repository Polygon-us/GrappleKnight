using Enemies.BasicEnemy.StateMachine.Bases;
using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.StateDecisions
{
    [CreateAssetMenu(fileName = "New AITimeInStateDecision", menuName = "AI/Decisions/AITimeInStateDecision", order = 0)]
    public class AITimeInStateDecision : StateDecision
    {
        [Header("Parameters")] 
        [SerializeField] private float minTimeInState;
        [SerializeField] private float maxTimeInState;

        private float resultTimeInState;
        private float currentTime;
        
        public override void InitDecision(Bases.StateMachine machineReference)
        {
            StateCondition = false;
            
            resultTimeInState = Random.Range(minTimeInState, maxTimeInState);
            currentTime = resultTimeInState;
        }

        public override void UpdateDecision()
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0 && !StateCondition)
                StateCondition = true;
        }
    }
}
