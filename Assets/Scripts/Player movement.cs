using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Climbing")]

    [SerializeField] private float climbingSpeed = 3f;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool stopClimbing;
    [SerializeField] private float walkVerticalSpeed = 7f;
    [SerializeField] private LayerMask checkLimitStair;
    private float _inicialGravity;
    private RaycastHit2D _checkStairLimit;
    private Vector2 verticalMovement;

    public PlatformEffector2D _platformEffector;
    public BoxCollider2D _boxColliderStairStop;
    
    [Header("HorizontalMovement")]

    [SerializeField] private float walkHorizontalSpeed = 9f;
    private Vector2 horizontalMovement;

    [Header("Jump")]

    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private LayerMask checkFloorMask;
    private float raycastLength = 1.01f;
    private RaycastHit2D _checkFloor;

    private Transform _myTransform;
    private Rigidbody2D _myrygidbody;
    private CapsuleCollider2D _myCapsuleCollider;
    private void Start()
    {
        _myrygidbody = GetComponent<Rigidbody2D>();
        _myTransform = GetComponent<Transform>();
        _myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        _inicialGravity = _myrygidbody.gravityScale;
    }
    private void FixedUpdate()
    {
        HorizontalMovement();
        Climbing();
        VerticalMovement();
    }
    private void Update()
    {
        Jump();
        OnGround();
    }

    private void VerticalMovement()
    {
        verticalMovement.y = Input.GetAxis("Vertical");
        if ((_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Stair")))) 
        {
            transform.Translate(climbingSpeed * Time.fixedDeltaTime * Vector2.up * verticalMovement);
        }
    }
    private void HorizontalMovement()
    {
        horizontalMovement.x = Input.GetAxis("Horizontal");
        transform.Translate(walkHorizontalSpeed * Time.fixedDeltaTime * Vector2.right * horizontalMovement);
    }

    public void Jump()
    {
        if (_checkFloor.collider != null && Input.GetKeyDown(KeyCode.Space))
        {

          _myrygidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void OnGround()
    {
         _checkFloor = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, checkFloorMask);
        
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down * raycastLength), Color.red);
    }
    public void Climbing()
    {
        if ((verticalMovement.y != 0 || isClimbing) && (_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Stair"))))
            {
                Vector2 climbingVelocity = new Vector2(_myrygidbody.velocity.x, verticalMovement.y * climbingSpeed);
                _myrygidbody.velocity = climbingVelocity;
                _myrygidbody.gravityScale = 0;
                isClimbing = true;
                Debug.Log(" Estoy Escalando");
            
        }
         
        else 
            {
               _myrygidbody.gravityScale = _inicialGravity;
                isClimbing = false;
                Debug.Log("no estoy escalando");
            
        }
        if ((verticalMovement.y < 0) && (_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("StairStop"))))
        {
            _platformEffector.enabled = false;
            _boxColliderStairStop.enabled = false;
            Debug.Log("estoy entrando");
        }
        else if ((_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Floor"))))
        {
            _platformEffector.enabled = true;
            _boxColliderStairStop.enabled = true;
        }
    }
}

