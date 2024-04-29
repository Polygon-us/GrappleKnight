using System;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected ScreenType screenType;

    public event Action<Enum> OnShowScreen;

    public event Action<ScreenType,Enum> OnChangeScreen;
    
    public event Action<Enum> OnNextSceneAction;
    
    public ScreenType GetScreenType()
    {
        return screenType;
    }

    protected void CallNextScene(Enum sceneName)
    {
        OnNextSceneAction?.Invoke(sceneName);
    }
    protected void CallScreen(Enum nextScreen)
    {
        OnShowScreen?.Invoke(nextScreen);
    }
    protected void CallChangeScreen(Enum nextScreen)
    {
        OnChangeScreen?.Invoke(screenType,nextScreen);
    }
    
    public virtual void SetComponents(params object[] components)
    {

    }

    public virtual void Show()
    {
        
    }
    
    public virtual void Hide()
    {
        
    }
}