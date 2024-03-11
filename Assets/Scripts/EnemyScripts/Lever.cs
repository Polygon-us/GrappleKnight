using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private Door _door;
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _door = GetComponentInChildren<Door>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boomerang"))
        {
            DoAnimations();
        }
    }

    private void DoAnimations()
    {
        _animator.SetTrigger("TurnOnLever");
        _door.DoOpenAnimation();
    }
}