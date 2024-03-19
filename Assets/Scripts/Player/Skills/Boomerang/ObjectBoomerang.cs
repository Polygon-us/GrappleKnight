using System;
using System.Collections;
using UnityEngine;

public class ObjectBoomerang : MonoBehaviour
{
    private Action _senderAction;
    private Action _receiverAction;

    [SerializeField] private float _shakeForceHook = 1f;

    public void Init(Action receiverAction)
    {
        _senderAction = receiverAction;
        _senderAction += _receiverAction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger != true)
        {
            _senderAction.Invoke();
           
        }
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.ReduceEnemyLife(10);
        }
        if (other.CompareTag("Enemy"))
        {
            CinemachineController.Instance.MoveCamera(_shakeForceHook, 5, 0.5f);

            Rigidbody2D _enemyRigidbody = other.GetComponent<Rigidbody2D>();
            SpriteRenderer _damageColor = other.GetComponent<SpriteRenderer>();
            other.GetComponent<EnemyStateController>().ChangeCurrentState(EnemyStateEnum.Idle);
            float forceSign = Mathf.Sign(other.transform.position.x-transform.position.x);
            Vector3 _diagonalForce = new Vector3(1f * forceSign, 1f, 0f).normalized * 10;
            _enemyRigidbody.AddForce(_diagonalForce, ForceMode2D.Impulse);
            StartCoroutine(DamageIndicator(_damageColor));
        }
        if (other.TryGetComponent(out Boss boss))
        {
            boss.ReduceEnemyLife(10);
        }
    }
    private IEnumerator DamageIndicator(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.03f);
        spriteRenderer.color = Color.red;
    }
    public void UnsubscribeAction()
    {
        _senderAction -= _receiverAction;
    }
    
}
