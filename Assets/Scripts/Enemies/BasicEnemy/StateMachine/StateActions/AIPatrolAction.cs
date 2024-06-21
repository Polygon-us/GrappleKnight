using Enemies.BasicEnemy.StateMachine.Bases;
using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.StateActions
{
    public class AIPatrolAction : StateAction
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform playerTrans;
        
        public override void UpdateAction()
        {
            playerTrans.position = new Vector2(playerTrans.position.x + speed * Time.deltaTime, playerTrans.position.y);
        }
    }
}
