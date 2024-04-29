using UnityEngine;

public class MainController : BaseController
{
    [SerializeField] private MainView mainView;
    private void Awake()
    {
        mainView.onStart += CallNextScene;
        mainView.onExitGame += ExitGame;
        mainView.Init();
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}