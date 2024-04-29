using System;
using UnityEngine.UI;

[Serializable]
public class NavigationButton<T>    //DavidT: Si no necesitamos tipos distintos a Enum (Que puede ser SceneEnum) se debe cambiar -> NavigationButton<SceneEnum>
{
    public Button button; //DavidT: Renombrar para más claridad y que no sea redundante con el nombre de la clase -> buttonGameObject
    public T navigation; //DavidT: Renombrar por claridad -> 
}