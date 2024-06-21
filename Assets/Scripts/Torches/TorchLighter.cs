using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLighter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer torch;
    [SerializeField] private BoxCollider2D torchCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            torch.enabled = true;
        }
    }

}
