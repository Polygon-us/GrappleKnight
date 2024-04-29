using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private MasterSceneControllers _masterSceneControllers;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SceneName firstScene;
    
     private void Start()
     { 
       
        UIManager.OnSetControllers += NextStep;
        _uiManager.Configure(sceneController);
        //sceneController.AddAction(SceneName.Main,ConfigureMain);
        
        sceneController.ChangeScene(firstScene);
       

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
