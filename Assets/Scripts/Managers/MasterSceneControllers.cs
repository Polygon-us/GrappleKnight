using UnityEngine;

public class MasterSceneControllers : MonoBehaviour
{
    [SerializeField] private BaseController[] _abstractBaseControllers;
    
    public BaseController[] GetControllers()
    {
        return _abstractBaseControllers;
    }
}