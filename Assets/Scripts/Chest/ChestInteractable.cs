using System;
using UnityEngine;

public class ChestInteractable : MonoBehaviour
{
    [Header("Animation Keyboard")]
    [SerializeField] private RectTransform _keyboardSprite;
    [SerializeField] private GameObject chest;
    
    private int _moveDistance = 1;
    
    private bool _isPlayerInRange = false;

    public GameObject Chest { get => chest;}

    public event Action OnChestPressed;

    private void Awake()
    {
           // chest = GetComponent<GameObject>();
    }

    private void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OnChestPressed?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
                _keyboardSprite.gameObject.SetActive(true);
                _isPlayerInRange = true;
                AnimateKeyboardKeySprite();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInRange = false;

            //Cancel the animation and deactivate the keyboard key sprite

            _keyboardSprite.gameObject.SetActive(false);
            LeanTween.cancel(_keyboardSprite.gameObject);

            //here the chest is open
        }
    }

    private void AnimateKeyboardKeySprite()
    {
        Debug.Log("Animando");

        //LeanTween for animating keyboard key press and release in a loop
        LeanTween.moveY(_keyboardSprite, _keyboardSprite.anchoredPosition.y + (_moveDistance * -0.5f), 1)
           .setEase(LeanTweenType.easeInOutSine)
           .setLoopPingPong();
    }
}
