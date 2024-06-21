using System;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    [SerializeField] private string message;

    private bool entry;

    private BoxCollider2D _boxCollider;
    public static event Action<string> OnCollision;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            if (entry == false) 
            {
                OnCollision?.Invoke(message);
                entry = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollision?.Invoke(null);
        }
    }
}
