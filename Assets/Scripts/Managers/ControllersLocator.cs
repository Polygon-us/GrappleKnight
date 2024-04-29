using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllersLocator : MonoBehaviour
{
    [SerializeField] private BaseController[] _abstractBaseControllers;

    private void Start()
    {
        UIManager.SetControllers(GetController());
    }

    private Dictionary<ScreenType, BaseController> GetControllers()
    {
        Dictionary<ScreenType,BaseController> controllers = new();
        foreach (BaseController item in _abstractBaseControllers)
        {
            controllers.Add(item.GetScreenType(),item);
        }
        return controllers;
    }

    private List<BaseController> GetController()
    {
        return _abstractBaseControllers.ToList();
    }
}