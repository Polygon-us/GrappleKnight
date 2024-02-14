using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpForce = 10f;
    public float raycastLength = 1.1f;
    public bool doMovementVertical = false;

    [SerializeField] private LayerMask checkFloor;
    public RaycastHit hit;
    private Transform _myTransform;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _myTransform = GetComponent<Transform>();
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
        if (doMovementVertical == true)
        {
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.up * verticalMovement);
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.right * horizontalMovement);
        }
        else
        {
            transform.Translate(walkSpeed * Time.deltaTime * Vector2.right * horizontalMovement);
        }

    }

    private void DoJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
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
}

