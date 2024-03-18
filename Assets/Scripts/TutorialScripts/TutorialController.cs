using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private TMP_Text messageField;

    [SerializeField] private RectTransform messageRect;

    private Vector3 position;

    private int? tween;

    private void Awake()
    {
        ShowText.OnCollision += OnCollisionHandler;

        position = messageRect.anchoredPosition;
    }

    private void OnCollisionHandler(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            messageField.text = string.Empty;

            messageRect.anchoredPosition = position;
        }
        else
        {
            messageField.text = message;

            if (tween.HasValue)
            {
                LeanTween.cancel(messageField.gameObject, tween.Value);

                messageRect.anchoredPosition = position;
            }

            tween = LeanTween.moveY(messageField.gameObject, 600, 1f).setEase(LeanTweenType.easeOutBounce).uniqueId;
        }
    }
}
