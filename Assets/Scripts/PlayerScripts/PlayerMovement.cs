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
        // Calcula la velocidad del movimiento
        Vector2 targetVelocity = new Vector2(input * speed, _myrigidbody.velocity.y);

        // Aplica la velocidad limitada
        targetVelocity.x = Mathf.Clamp(targetVelocity.x, -maxSpeed, maxSpeed);
        targetVelocity.y = _myrigidbody.velocity.y; // Mantén la velocidad vertical

        // Calcula el cambio en la velocidad
        Vector2 velocityChange = targetVelocity - _myrigidbody.velocity;

        // Aplica una fuerza para mantener la velocidad actual
        _myrigidbody.AddForce(velocityChange * inertiaMultiplier, ForceMode2D.Force);
    }

    public void CaptureMovementInput()
    {
         input = _moveAction.actions["Move"].ReadValue<float>();
        Debug.Log("Input Value: " + input);

    }
}

