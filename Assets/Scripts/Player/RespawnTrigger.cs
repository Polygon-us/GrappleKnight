using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public static event Action<Vector3> OnCollision;

    private Vector3 _respawnPosition;
    void Start()
    {
        _respawnPosition = transform.position;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("aY COLISIONE");
            OnCollision?.Invoke(_respawnPosition);
        }
    }
    
}
