using UnityEngine;
using UnityEngine.Serialization;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float _jumpHeight;
    [SerializeField][Range(1,100)]private int _percentDamage;
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.TryGetComponent(out ILife life) && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            life.ReduceLife(_percentDamage);
            Rigidbody2D _rigidbody2D = other.rigidbody;
            float velocity = Mathf.Sqrt(_jumpHeight * (Physics2D.gravity.y * _rigidbody2D.gravityScale) * -2);
            velocity *= _rigidbody2D.mass; 
            _rigidbody2D.AddForce(Vector2.up*velocity, ForceMode2D.Impulse);
            
        }
    }
}