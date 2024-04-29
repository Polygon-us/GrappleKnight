using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    private static List<BaseController> _controllersContainer;
    //private static AbstractBaseController[] _masterSceneControllers;

    public static event Action OnSetControllers;
    private SceneController _sceneController;
    
    private void Awake()
    {
        OnSetControllers += AddListeners;
    }

    public void Configure(SceneController sceneController)
    {
        _sceneController = sceneController;
    }

    public static void SetControllers(List<BaseController> controllers)
    {
        _controllersContainer = controllers;
        OnSetControllers?.Invoke();
    }
    
    private void AddListeners()
    {
        foreach (BaseController item in _controllersContainer)
        {
            item.OnNextSceneAction += SceneChange;
            item.OnShowScreen += ShowScreen;
            item.OnChangeScreen += ScreenChange;
        }
    }

    public BaseController GetController(ScreenType screenType)
    {
        return _controllersContainer.FirstOrDefault(t => t.GetScreenType() == screenType);
    }

    public void ShowScreen(Enum nextScreen)
    {
        ScreenType nextScreenType = (ScreenType)nextScreen;
        
        _controllersContainer.FirstOrDefault(t => t.GetScreenType() == nextScreenType).Show();
        
    }
    
    private void ScreenChange(ScreenType currentScreen, Enum nextScreen)
    {
        ScreenType nextScreenType = (ScreenType)nextScreen;
        
        _controllersContainer.FirstOrDefault(t => t.GetScreenType() == currentScreen).Hide();
        _controllersContainer.FirstOrDefault(t => t.GetScreenType() == nextScreenType).Show();
        
    }
    
    private void SceneChange(Enum sceneName)
    {
        _sceneController.ChangeScene(sceneName);
    }
}

