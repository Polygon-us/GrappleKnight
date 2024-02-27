using UnityEngine;

public class EnemyHuntState : IState
{

    private Transform _enemyTransform;
    private Transform _playerTransform;

    private Rigidbody2D _enemyRigidbody;

    private BoxCollider2D _persecutorCollider;

    private bool _isHuntingMode;
    
    private float _walkSpeed = 5f;

    private Vector2 playerDirection;
    public EnemyHuntState(BoxCollider2D persecutorCollider, Rigidbody2D enemyRigidbody, Transform enemyTransform, Transform playerTransform, bool isHuntingMode)
    {
        _persecutorCollider = persecutorCollider;
        _enemyRigidbody = enemyRigidbody;
        _enemyTransform = enemyTransform;
        _playerTransform = playerTransform;
        _isHuntingMode = isHuntingMode;
    }

    public void HuntPlayer()
    {
         playerDirection = (_playerTransform.position - _enemyTransform.position).normalized;

        Vector2 moveDirection = new Vector2(playerDirection.x, 0f).normalized;

        _enemyRigidbody.velocity = moveDirection * _walkSpeed;
    }

    //public void CatchThePlayer()
    //{
    //    if (_persecutorCollider.IsTouchingLayers(LayerMask.GetMask("Player")))  
    //    {

    //    }
    //}

    public bool DoState(out EnemyStateEnum enemyStateEnum)
    {
            HuntPlayer();

            enemyStateEnum = EnemyStateEnum.Hunt;
            return true;
    }
}
