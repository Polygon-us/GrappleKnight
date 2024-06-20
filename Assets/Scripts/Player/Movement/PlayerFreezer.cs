using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerFreezer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D playerRigidBody;
    [SerializeField] PlayerLife playerLife;
    [SerializeField] BossZoneTwo bossZone;
    [SerializeField] Boss boss;

    [Header("Freeze Variables")]
    private float _freezeDuration = 2;

    public bool freeze;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (freeze)
            {
                StartCoroutine(Freeze());
            }
        }
    }

    private IEnumerator Freeze()
    {
        RigidbodyConstraints2D originalConstraints = playerRigidBody.constraints;

        playerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(_freezeDuration);

        playerRigidBody.constraints = originalConstraints;

        freeze = false;

        boss.InitialState();
    }
}
