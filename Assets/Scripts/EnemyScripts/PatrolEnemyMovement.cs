using UnityEngine;

public class PatrolEnemyMovement : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField] private float walkHorizontalSpeed = 4f;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
    }
    void Update()
    {
        
    }
    private void HorizontalMovement()
    {

        transform.Translate(walkHorizontalSpeed * Time.fixedDeltaTime * Vector2.right);
        
    }

    
}
