using UnityEngine;

namespace UI
{
    public class SettingsController : BaseController
    {
        [SerializeField] private SettingsView settignsView;
        
        private void Awake()
        {
            settignsView?.ExitButton?.onClick.AddListener(ExitGame);

            GameManager.OnGamePause += paused =>
            {
                if(paused) Show();
                else Hide();
            };
        }

        public override void Show()
        {
            base.Show();

            if (settignsView)
            {
                settignsView.MainCanvasGroup.alpha = 1;
                settignsView.MainCanvasGroup.interactable = true;
                settignsView.MainCanvasGroup.blocksRaycasts = true;
            }
        }
        
        public override void Hide()
        {
            base.Hide();

            if (settignsView)
            {
                settignsView.MainCanvasGroup.alpha = 0;
                settignsView.MainCanvasGroup.interactable = false;
                settignsView.MainCanvasGroup.blocksRaycasts = false;
            }
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
