using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    [Header("HorizontalMovement")]

    [SerializeField] private float force = 10f;
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
        if (Mathf.Abs(_myrigidbody.velocity.x) <= 2f)
        {
            _myrigidbody.AddForce(force * new Vector2(input, 0f), ForceMode2D.Force);
        }
    }

    public void CaptureMovementInput()
    {
         input = _moveAction.actions["Move"].ReadValue<float>();
        Debug.Log("Input Value: " + input);

    }
}

