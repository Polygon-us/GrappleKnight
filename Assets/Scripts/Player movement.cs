using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        BasicMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoJump();
        }
    }

    private void BasicMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalMovement, verticalMovement) * walkSpeed;
        _rb.velocity = new Vector2(movement.x, _rb.velocity.y); 
    }

    private void DoJump()
    {
        if (IsGrounded())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }
}
