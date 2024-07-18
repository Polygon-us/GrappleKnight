using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private MasterSceneControllers _masterSceneControllers;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SceneName firstScene;

    public static event Action<bool> OnGamePause;

    private bool _enablePause = false;

    private void Awake()
    {
        sceneController.OnLvlStarted += () =>
        {
            if (!_enablePause) _enablePause = true;

            Time.timeScale = 1;
        };
    }

    private void Start()
    {
        UIManager.OnSetControllers += NextStep;
        _uiManager.Configure(sceneController);
        //sceneController.AddAction(SceneName.Main,ConfigureMain);

        sceneController.ChangeScene(firstScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_enablePause) return;

            bool pause = Time.timeScale > 0;
            Time.timeScale = pause ? 0 : 1;

            OnGamePause?.Invoke(pause);
        }
    }

    private void NextStep()
    {
        sceneController.CallMethod();
    }

    private void ConfigureMain()
    {
        //_uiManager.ShowScreen();
    }
}