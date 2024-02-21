using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("HorizontalMovement")]

    [SerializeField] private float walkHorizontalSpeed = 9f;
    private Vector2 horizontalMovement;

 
    private CapsuleCollider2D _myCapsuleCollider;
    private void Start()
    {
        _myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
    }

    private void Update()
    {
        
    }

    private void HorizontalMovement()
    {
        horizontalMovement.x = Input.GetAxis("Horizontal");
        transform.Translate(walkHorizontalSpeed * Time.fixedDeltaTime * Vector2.right * horizontalMovement);
    }
}

