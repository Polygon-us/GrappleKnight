using UnityEngine;

public class CursorLock : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; 
        Cursor.visible = false; 
    }

    void OnApplicationFocus(bool hasFocus)
    {
       
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Confined; 
            Cursor.visible = false; 
        }
    }
}
