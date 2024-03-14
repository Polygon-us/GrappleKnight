using System;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    public static event Action<string> OnCollision;


    [SerializeField] private string message;


    private BoxCollider2D m_BoxCollider;

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollision?.Invoke(message);
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
