using UnityEngine;
using UnityEngine.Events;

public class BossZoneTwo : MonoBehaviour
{
    [SerializeField] private UnityEvent _unityEvent;

    [SerializeField]private bool _isOnTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_isOnTrigger)
            {
                _isOnTrigger = true;
                _unityEvent.Invoke();
            }
        }
    }
}