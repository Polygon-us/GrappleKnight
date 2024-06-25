using Enemies.BasicEnemy.StateMachine.Bases;
using Enemies.BasicEnemy.Weapons;
using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.StateActions
{
    [CreateAssetMenu(fileName = "New AIAttackAction", menuName = "AI/Actions/AIAttackAction")]
    public class AIAttackAction : StateAction
    {
        private AIBrain _aiBrain;
        private HandleWeapon _handleWeapon;
        
        public override void InitAction(Bases.StateMachine machineReference)
        {
            _aiBrain = (AIBrain)machineReference;

            if (_aiBrain)
            {
                _handleWeapon = _aiBrain.ReferenceController.HandleWeapon;
            }
        }

        public override void UpdateAction()
        {
            _handleWeapon.UseWeapon();
        }
    }
}
