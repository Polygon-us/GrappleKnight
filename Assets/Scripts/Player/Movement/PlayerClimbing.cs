using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    [Header("isClimbing")]
    [SerializeField] private float climbingSpeed = 5f;

    [SerializeField] private LayerMask checkLimitStair;
        
    [SerializeField] private CapsuleCollider2D _myCollider;

    [SerializeField] private PlatformEffector2D _platformEffector;

    public bool isClimbing = false;

    private float _inicialGravity;

    private RaycastHit2D _checkStairLimit;

    private Vector2 _velocityOnClimb;
    private Vector2 verticalMovement;

    [Header("References")]
    [SerializeField] private Rigidbody2D _myrygidbody;
    [SerializeField] private BoxCollider2D _boxColliderStairStop;
    
    private Transform _myTransform;

    private void Start()
    {
        _myrygidbody = GetComponent<Rigidbody2D>();
        _myTransform = GetComponent<Transform>();
        _myCollider = GetComponent<CapsuleCollider2D>();
        _inicialGravity = _myrygidbody.gravityScale;
    }
    private void FixedUpdate()
    {
        Climbing();
        VerticalMovement();
    }
  
    private void VerticalMovement()
    {
        verticalMovement.y = Input.GetAxis("Vertical");
        if ((verticalMovement.y != 0 && _myCollider.IsTouchingLayers(LayerMask.GetMask("Stair"))))
        {
            transform.Translate(climbingSpeed * Time.fixedDeltaTime * Vector2.up * verticalMovement);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stair") || other.CompareTag("StairStop"))
        {
            _platformEffector = other.transform.Find("StairTop").GetComponent<PlatformEffector2D>();
            _boxColliderStairStop = other.transform.Find("StairTop").GetComponent<BoxCollider2D>();
        }
    }
    public void Climbing()
    {
        _velocityOnClimb = new Vector2(_myrygidbody.velocity.x, 0);
        if ((verticalMovement.y != 0 || isClimbing) && (_myCollider.IsTouchingLayers(LayerMask.GetMask("Stair"))))
        {
            _myrygidbody.gravityScale = 0;
            _myrygidbody.velocity = _velocityOnClimb;
            isClimbing = true;
        }
        else
        {
            _myrygidbody.gravityScale = _inicialGravity;
            isClimbing = false;
        }
        if ((verticalMovement.y < 0) && (_myCollider.IsTouchingLayers(LayerMask.GetMask("StairStop"))))
        {
            _myrygidbody.gravityScale = 0;
            _myrygidbody.velocity = Vector2.zero;
            isClimbing = true;
            _boxColliderStairStop.enabled = false;
        }
       
        if ((_myCollider.IsTouchingLayers(LayerMask.GetMask("Floor"))))
        {
            _platformEffector.enabled = true;
            _boxColliderStairStop.enabled = true;
        }

    }
}
