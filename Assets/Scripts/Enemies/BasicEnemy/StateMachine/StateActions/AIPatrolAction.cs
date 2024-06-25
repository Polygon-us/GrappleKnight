using Enemies.BasicEnemy.StateMachine.Bases;
using UnityEngine;

namespace Enemies.BasicEnemy.StateMachine.StateActions
{
    [CreateAssetMenu(fileName = "New AIPatrolAction", menuName = "AI/Actions/AIPatrolAction", order = 0)]
    public class AIPatrolAction : StateAction
    {
        [Header("Movement")]
        [SerializeField] private float speed;
        
        [Header("Conditions")]
        [SerializeField] private bool avoidFalling;
        [SerializeField] private bool changeDirection;

        private AIBrain _aiBrain;
        
        //Utility
        private Transform _playerTransform;
        private float _currentSpeed;
        
        public override void InitAction(Bases.StateMachine machineReference)
        {
            _aiBrain = (AIBrain)machineReference;

            if (_aiBrain)
            {
                _playerTransform = _aiBrain.ReferenceController.gameObject.transform;
            }

            _currentSpeed = speed;
        }

        public override void UpdateAction()
        {
            if(!_aiBrain) return;
            
            if(!_aiBrain.ReferenceController.Grounded) return;
            
            CheckDirection();
            
            CheckForWalk();
            
            Move();
        }

        private void CheckDirection()
        {
            if (changeDirection)
            {
                if (avoidFalling)
                {
                    if (!_aiBrain.ReferenceController.CanWalk)
                    {
                        _aiBrain.ReferenceController.ChangeDirection();
                        return;
                    }
                }
                
                if (_aiBrain.ReferenceController.Blocked)
                {
                    _aiBrain.ReferenceController.ChangeDirection();
                }
            }
        }
        
        private void CheckForWalk()
        {
            if ((avoidFalling && !_aiBrain.ReferenceController.CanWalk) || _aiBrain.ReferenceController.Blocked)
            {
                _currentSpeed = 0;
                return;
            }
            
            _currentSpeed = speed;
        }

        
        private void Move()
        {
            _playerTransform.position += _playerTransform.right * (_currentSpeed * Time.deltaTime);
        }
    }
}
