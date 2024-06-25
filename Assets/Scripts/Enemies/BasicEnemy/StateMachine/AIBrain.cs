using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine
{
    public class AIBrain : Bases.StateMachine
    {
        [SerializeField] private List<AIState> states = new List<AIState>();

        public Action<string> _OnChangeStateRequired;

        public EnemyController ReferenceController { get; private set; }
        
        public void InitMachine(EnemyController enemyController)
        {
            ReferenceController = enemyController;
            
            _OnChangeStateRequired += ParseStateChange;
            
            foreach (AIState element in states)
            {
                element.Init(this);
            }
            
            ChangeState(states.ElementAt(0), true);
        }

        private void Update()
        {
            UpdateMachine();
        }

        private void ParseStateChange(string stateName)
        {
            ChangeState(states.FirstOrDefault(state => state.stateName == stateName), true);
        }
    }
}
