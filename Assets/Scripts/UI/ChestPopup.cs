using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestPopup : MonoBehaviour
{
    [Header("References")]
    private ChestInteractable chestInteractable;
    
    [Header("UI")]
    [SerializeField] private Image content;
    [SerializeField] private TMP_Text title;
    [SerializeField] private Button close;
    [SerializeField] private CanvasGroup _chestPanel;

    [Header("Animation Parameters")]
    [SerializeField] private int _duration;
    [SerializeField] private Transform chestDestinationPosicion;

    private Vector3 initialPosition;

    private void Awake()
    {
        chestInteractable = GetComponent<ChestInteractable>();

        chestInteractable.OnChestPressed += ShowChestContent;
    }

    void Start()
    {
        initialPosition = content.transform.position;
        Debug.Log(initialPosition);
    }

    private void ShowChestContent()
    {
        close.onClick.AddListener(CloseChestContent);

        _chestPanel.alpha = 1.0f;
        LeanTween.move(content.gameObject, chestDestinationPosicion, _duration).setEaseOutExpo();
    }

    private void CloseChestContent()
    {
        _chestPanel.alpha = 0;
        LeanTween.move(content.gameObject, initialPosition, _duration).setEaseOutExpo();

        LeanTween.cancel(content.gameObject);

        gameObject.SetActive(false);

        close.onClick.RemoveListener(CloseChestContent);
    }
}

