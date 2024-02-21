using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]

    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private LayerMask checkFloorMask;
    private float raycastLength = 1.01f;
    private RaycastHit2D _checkFloor;

    private Transform _myTransform;
    private Rigidbody2D _myrygidbody;
    void Start()
    {
        _myrygidbody = GetComponent<Rigidbody2D>();
        _myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Jump();
        OnGround();
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

}
