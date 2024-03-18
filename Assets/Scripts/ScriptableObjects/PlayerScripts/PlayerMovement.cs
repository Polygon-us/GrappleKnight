using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    [Header("HorizontalMovement")]

    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float inertiaMultiplier = 10f;
    private Vector2 horizontalMovement;

 
    private Rigidbody2D _myrigidbody;
    private PlayerInput _moveAction;
    private float input;
    private void Start()
    {
        _myrigidbody = GetComponent<Rigidbody2D>();
        _moveAction = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
    }

    private void Update()
    {
        CaptureMovementInput();
    }

    private void HorizontalMovement()
    {
        Vector2 targetVelocity = new Vector2(input * speed, _myrigidbody.velocity.y);

        targetVelocity.x = Mathf.Clamp(targetVelocity.x, -maxSpeed, maxSpeed);
        targetVelocity.y = _myrigidbody.velocity.y; 

        Vector2 velocityChange = targetVelocity - _myrigidbody.velocity;

        _myrigidbody.AddForce(velocityChange * inertiaMultiplier, ForceMode2D.Force);
    }

    public void CaptureMovementInput()
    {
         input = _moveAction.actions["Move"].ReadValue<float>();


    }
}

