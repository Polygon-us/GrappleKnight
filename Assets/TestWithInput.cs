using UnityEngine;

public class TestWithInput : MonoBehaviour
{
    private void Awake()
    {
        InputManager d = new InputManager(new PlayerInputAction());
      
    }
}