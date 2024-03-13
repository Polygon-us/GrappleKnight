using System;
using System.Collections;
using UnityEngine;

public class ObjectBoomerang : MonoBehaviour
{
    private Action _senderAction;
    private Action _receiverAction;

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
            other.GetComponent<EnemyStateController>().PushEnemy();
            float direction = Mathf.Sign(other.transform.position.x - transform.position.x);
            StartCoroutine(CPush(other.GetComponent<Rigidbody2D>(),direction));
            other.GetComponent<Rigidbody2D>().AddForce(Vector2.right*direction*10,ForceMode2D.Impulse);
            enemy.ReduceEnemyLife(10);
        }
        if (other.TryGetComponent(out Boss boss))
        {
            boss.ReduceEnemyLife(10);
        }
    }
    
    public void UnsubscribeAction()
    {
        _senderAction -= _receiverAction;
    }

    IEnumerator CPush(Rigidbody2D rigidbody2D, float direction)
    {
        yield return new WaitForSeconds(0.2f);
        rigidbody2D.AddForce(Vector2.right*direction*10,ForceMode2D.Impulse);
    }
}
