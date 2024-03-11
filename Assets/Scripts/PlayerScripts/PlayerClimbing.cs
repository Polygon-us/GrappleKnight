using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    [Header("isClimbing")]

    [SerializeField] private float climbingSpeed = 5f;
    [SerializeField] private bool isClimbing;
    [SerializeField] private LayerMask checkLimitStair;
    [SerializeField] private CapsuleCollider2D _myCapsuleCollider;
    [SerializeField] private PlatformEffector2D _platformEffector;


    private float _inicialGravity;
    private RaycastHit2D _checkStairLimit;
    private Vector2 verticalMovement;

    [Header("References")]

    private Transform _myTransform;
    [SerializeField] private Rigidbody2D _myrygidbody;
    [SerializeField] private BoxCollider2D _boxColliderStairStop;
    private void Start()
    {
        _myrygidbody = GetComponent<Rigidbody2D>();
        _myTransform = GetComponent<Transform>();
        _myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        _inicialGravity = _myrygidbody.gravityScale;
    }
    private void FixedUpdate()
    {
        Climbing();
        VerticalMovement();
        
    }
    void Update()
    {
    }
  
    private void VerticalMovement()
    {
        verticalMovement.y = Input.GetAxis("Vertical");
        if ((_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Stair"))))
        {
            transform.Translate(climbingSpeed * Time.fixedDeltaTime * Vector2.up * verticalMovement);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stair"))
        {
            _platformEffector = other.transform.Find("StairTop").GetComponent<PlatformEffector2D>();
            _boxColliderStairStop = other.transform.Find("StairTop").GetComponent<BoxCollider2D>();
        }
    }
    public void Climbing()
    {
        if ((verticalMovement.y != 0 || isClimbing) && (_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Stair"))))
        {
            _myrygidbody.gravityScale = 0;
            isClimbing = true;
            Debug.Log("cero gravedad");
        }
        else
        {
            _myrygidbody.gravityScale = _inicialGravity;
            isClimbing = false;
        }
        if ((verticalMovement.y < 0) && (_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("StairStop"))))
        {
            _boxColliderStairStop.enabled = false;
        }
        else if ((_myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Floor"))))
        {
            _platformEffector.enabled = true;
            _boxColliderStairStop.enabled = true;
        }

    }
}
