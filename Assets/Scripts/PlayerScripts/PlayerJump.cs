using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]

    [SerializeField] private float jumpForce = 5f;
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
        OnGround();
    }
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (_checkFloor.collider != null && callbackContext.performed)
        {
            _myrygidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            Debug.Log(callbackContext.phase);
        }
    }
    public void OnGround()
    {
        _checkFloor = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, checkFloorMask);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down * raycastLength), Color.red);
    }

}
