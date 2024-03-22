using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossZone : MonoBehaviour
{
    [SerializeField]private GameObject _door;
    [SerializeField]private EnemyStateController _controller;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(CloseDoor());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _door.SetActive(false);
            _controller.StopStates();
        }
    }
    private IEnumerator CloseDoor()
    {
        _door.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        _door.SetActive(true);
    }
}