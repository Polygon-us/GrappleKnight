using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 9f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float raycastLength = 1.01f;
    [SerializeField] private bool doMovementVertical = false;
    [SerializeField] private float climbingSpeed = 7f;
    [SerializeField] private LayerMask checkFloor;

    private Vector2 input;
    private Transform _myTransform;
    private Rigidbody2D _myrygidbody;

    private void Start()
    {
        _myrygidbody = GetComponent<Rigidbody2D>();
        _myTransform = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {

    }
    private void Update()
    {
        Movement();
        JumpIfOnGround();
    }


    private void Movement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        if (doMovementVertical)
        {
            transform.Translate(climbingSpeed * Time.deltaTime * Vector2.up * verticalMovement);
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.right * horizontalMovement);
            Climbing();
        }
        else
        {
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.right * horizontalMovement);
            _myrygidbody.gravityScale = 1f; 
        }

    }
    public void JumpIfOnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, checkFloor);

        if (hit.collider != null && Input.GetKeyDown(KeyCode.Space))
        {
            _myrygidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down * 1.1f), Color.red);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stair"))
        {
            doMovementVertical = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        doMovementVertical = false;
    }

    public void Climbing()
    {
        Vector2 climbingVelocity = new Vector2(_myrygidbody.velocity.x, input.y * climbingSpeed);
        _myrygidbody.velocity = climbingVelocity;

        if (doMovementVertical == true)
        {
            _myrygidbody.gravityScale = 0;
        }
        else if (doMovementVertical == false)
        {
            _myrygidbody.gravityScale = 1f;

        }
    }
}

