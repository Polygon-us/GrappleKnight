using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsView : MonoBehaviour
    {
        [Header("Canvas Group")]
        [SerializeField] private CanvasGroup mainCanvasGroup;

        [Header("Buttons")] 
        [SerializeField] private Button exitButton;

        public CanvasGroup MainCanvasGroup => mainCanvasGroup;
        public Button ExitButton => exitButton;
    }
}
