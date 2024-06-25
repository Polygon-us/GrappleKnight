using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.Bases
{
    public abstract class StateAction : ScriptableObject
    {
        public abstract void InitAction(StateMachine machineReference);
        public abstract void UpdateAction();
    }
}
