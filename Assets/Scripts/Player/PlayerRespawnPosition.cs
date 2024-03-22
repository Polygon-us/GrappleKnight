using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawnPosition : MonoBehaviour
{
    private Vector2 _playerPosition;

    void Start()
    {
        RespawnTrigger.OnCollision += SetPosition;
        _playerPosition = transform.position;
    }

    private void SetPosition(Vector3 position)
    {
        _playerPosition = position;
    }
    public void RespawnLastPosition()
    {
        transform.position = _playerPosition;
    }
}
