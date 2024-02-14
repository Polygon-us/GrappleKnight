using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpForce = 10f;
    public float raycastLength = 1.1f;
    public bool doMovementVertical = false;
    public float climbingSpeed = 3f;
    public float gravityValue;
    private Vector2 input;

    [SerializeField] private LayerMask checkFloor;
    public RaycastHit2D hit;
    private Transform _myTransform;
    private Rigidbody2D _myrygidbody;

    private void Start()
    {
        _myrygidbody = GetComponent<Rigidbody2D>();
        _myTransform = GetComponent<Transform>();
        gravityValue = _myrygidbody.gravityScale;
    }

    private void Update()
    {
        Movement();
        OnGround();
    }


    private void Movement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        if (doMovementVertical == true && verticalMovement != 0)
        {
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.up * verticalMovement);
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.right * horizontalMovement);
            Climbing();
        }
        else 
        {
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.right * horizontalMovement);
            _myrygidbody.gravityScale = 1f;;        
        }

    }

    private void DoJump()
        {
            if (Input.GetKey(KeyCode.Space))
                {
                  _myrygidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
                }
        }

    public void OnGround() 
        {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, checkFloor);

        if (hit.collider != null)
        {
            DoJump();
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down * 1.1f), Color.red);
        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stair"))
        {
            doMovementVertical = true;
            Debug.Log("entre");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        doMovementVertical = false;
        Debug.Log("sali");
    }

    public void Climbing()
    {
        Vector2 climbingVelocity = new Vector2(_myrygidbody.velocity.x, input.y * climbingSpeed);
        _myrygidbody.velocity = climbingVelocity;
        if (doMovementVertical == true)
        {
            _myrygidbody.gravityScale = 0;
            Debug.Log("cero gravedad");
        }
        else if (doMovementVertical == false) 
        {
            _myrygidbody.gravityScale = 1f;
            Debug.Log("gravedad");

        }
    }
}

