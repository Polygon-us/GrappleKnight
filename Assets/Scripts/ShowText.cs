using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TextMeshPro;
    private BoxCollider2D m_BoxCollider;
    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_TextMeshPro.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_TextMeshPro.enabled = true;
        }
            

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_TextMeshPro.enabled = false;
        }
    }
}
