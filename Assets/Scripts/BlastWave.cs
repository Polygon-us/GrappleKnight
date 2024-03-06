using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    public Action OnPlayerDamage;
    public bool touch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerDamage?.Invoke();
            touch = true;
        }
    }
}

public class ExampleBoss
{
    public BlastWave blast;

    public void DoBlast()
    {
        blast.OnPlayerDamage += SuccessHit;
    }

    private void SuccessHit()
    {
        Debug.Log("Player Hit");
    }
}