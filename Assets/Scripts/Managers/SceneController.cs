using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private string lastScene;
    private Dictionary<SceneName, Action> _methodsContainer = new();

    public event Action OnLvlStarted;

    public void AddAction(SceneName sceneName, Action action)
    {
        _methodsContainer.Add(sceneName,action);
    }
    public void ChangeScene(Enum sceneName)
    {
        if (lastScene != null)
        {
            SceneManager.UnloadSceneAsync(lastScene);
        }

        SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);

        int buildindex = SceneManager.GetSceneByName(sceneName.ToString()).buildIndex;
        
        if(buildindex > 1) OnLvlStarted?.Invoke();
        
        lastScene = sceneName.ToString();
    }
    
    public void CallMethod()
    {
        SceneName scene = (SceneName)Enum.Parse(typeof(SceneName),lastScene);
        if (_methodsContainer.ContainsKey(scene))
        {
            _methodsContainer[scene]();
        }
    }
}
