using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoomerangCollisionForce : MonoBehaviour
{
    [SerializeField]private float _knockback = 1f;
    private Vector3 _diagonalForce;

    private Rigidbody2D _enemyRigidbody;

    private SpriteRenderer _damageColor;


    private BoxCollider2D m_CollisionBody;
    void Start()
    {
        m_CollisionBody = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _enemyRigidbody = collision.transform.GetComponent<Rigidbody2D>();
            _damageColor = collision.transform.GetComponent<SpriteRenderer>();
            collision.transform.GetComponent<EnemyStateController>().ChangeCurrentState(EnemyStateEnum.Idle);
            StartCoroutine(DamageIndicator());
        }
    }
   

    private IEnumerator DamageIndicator()
    {
        _damageColor.color = Color.white;
        yield return new  WaitForSeconds(0.03f);
        _diagonalForce = new Vector3(-1f, 1f, 0f).normalized * _knockback;
        _enemyRigidbody.AddForce(_diagonalForce, ForceMode2D.Impulse);
        _damageColor.color = Color.red;
    } 
}
