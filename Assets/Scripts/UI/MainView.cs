using System;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField] private NavigationButton<SceneName> startButton;

    [SerializeField] private Button exitGameButton;
    
    public event Action<Enum> onStart;
    public event Action onExitGame;
    public void Init()
    {
        Addlisteners();
    }

    private void Addlisteners()
    {
        startButton.button.onClick.AddListener(()=>CallGame(startButton.navigation));
        exitGameButton.onClick.AddListener(CallExitGame);
    }

    private void CallExitGame()
    {
        onExitGame?.Invoke();
    }

    private void CallGame(SceneName startButtonNavigation)
    {
        onStart?.Invoke(startButtonNavigation);
    }
}