using UnityEngine;
using UnityEngine.InputSystem;

public class BoomerangSkill : ISkill
{
    private Transform _boomerangTransform;
    private Transform _transform;

    private Vector3 _newPosition;
    private Vector3 _createPosition;
    private Vector3 _positionOnThrow;
    private Vector3 _initialPosition;
    
    private float _boomerangMaxDistance;
    private float _boomerangSpeed;
    
    private Camera _mainCamera;
    private Mouse _mousePosition;

    private float _skillDuration;
    public BoomerangSkill(Transform transform, Transform boomerangTransform, float boomerangMaxDistance, float boomerangSpeed)
    {
        _transform = transform;
        _boomerangTransform = boomerangTransform;
        _boomerangMaxDistance = boomerangMaxDistance;
        _boomerangSpeed = boomerangSpeed;
        _mainCamera = Camera.main;
        _mousePosition = Mouse.current;
    }

    public void InitSkill()
    {
        _newPosition = _mousePosition.position.ReadValue();
        _newPosition = _mainCamera.ScreenToWorldPoint(_newPosition);

        _newPosition.z = 0;
        
        _positionOnThrow = _transform.position;

        Vector3 restPositions = _newPosition - _positionOnThrow;
        restPositions = restPositions.normalized*_boomerangMaxDistance;
        _newPosition = _positionOnThrow + restPositions;
        _initialPosition = _positionOnThrow;
        _skillDuration = -1;
        _boomerangTransform.localPosition = Vector3.zero;
    }

    public bool DoSkill()
    {
        _skillDuration += _boomerangSpeed*Time.deltaTime;
        float t = -(_skillDuration * _skillDuration) + 1;
        _boomerangTransform.position = Vector3.Lerp(_initialPosition, _newPosition, t);
        if (_skillDuration>=0)
        {
            _initialPosition = _transform.position;
        }
        if (t<0)
        {
            return false;
        }
        return true;
    }
    
    public void UndoSkill()
    {
       
    }
}

public static class CDebug
{
   static int lastDebugColor = 0;

   public static void Log(object message)
   {
         int randomLog = Random.Range(0, 9);
         switch (randomLog)
         {
             case 0:
                 LogRed(message);
                 break;
                case 1:
                 LogOrange(message);
                    break;
                case 2:
                 LogYellow(message);
                    break;
                case 3:
                 LogGreen(message);
                    break;
                case 4:
                 LogBlue(message);
                    break;
                case 5:
                 LogPurple(message);
                    break;
                case 6:
                 LogRandomColor(message);
                    break;
                case 7:
                 LogRainbowSingle(message);
                    break;
                case 8:
                 LogRainbowOneLine(message);
                    break;
                case 9:
                 LogRainbow(message);
                    break;
             
         }
   }
    public static void LogRed(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=red>"+message+"</color>");
#endif
    }
    
    public static void LogOrange(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=orange>"+message+"</color>");
#endif
    }
    public static void LogYellow(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=yellow>"+message+"</color>");
#endif
    }
    public static void LogGreen(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=green>"+message+"</color>");
#endif
    }
    
    public static void LogBlue(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=blue>"+message+"</color>");
#endif
    }
    
    public static void LogPurple(object message)
    {
#if UNITY_EDITOR
        Debug.Log("<color=purple>"+message+"</color>");
#endif
    }
   public static void LogRandomColor(object message)
   {
       string[] colors = new string[] {"red", "orange", "yellow", "green", "blue", "purple"};
       lastDebugColor = lastDebugColor >= colors.Length ? 0 : lastDebugColor;
       Debug.Log("<color="+colors[lastDebugColor]+">"+message+"</color>");
       lastDebugColor++;
   }
   public static void LogRainbowSingle(object message)
   {
    LogRainbowOneLine(message);
   }
   public static void LogRainbowOneLine(object message)
   {
       string messageToLog = "";
       
       foreach (var letter in message.ToString())
       {
           switch (lastDebugColor)
           {
               case 0:
                  messageToLog += "<color=red>"+letter+"</color>";
                   break;
               case 1:
                   messageToLog += "<color=orange>"+letter+"</color>";
                   break;
                case 2:
                    messageToLog += "<color=yellow>"+letter+"</color>";
                    break;
                case 3:
                    messageToLog += "<color=green>"+letter+"</color>";
                    break;
                case 4:
                    messageToLog += "<color=blue>"+letter+"</color>";
                    break;
                case 5:
                    messageToLog += "<color=purple>"+letter+"</color>";
                    break;
           }
              lastDebugColor++;
              if (lastDebugColor > 5)
              {
                    lastDebugColor = 0;
              }
       }
         Debug.Log(messageToLog);
   }
   public static void LogRainbow(object message)
   {
       switch (lastDebugColor)
       {
           case 0:
               Debug.Log("<color=red>"+message+"</color>");
               break;
           case 1:
               Debug.Log("<color=orange>"+message+"</color>");
               break;
           case 2:
               Debug.Log("<color=yellow>"+message+"</color>");
               break;
           case 3:
               Debug.Log("<color=green>"+message+"</color>");
               break;
           case 4:
               Debug.Log("<color=blue>"+message+"</color>");
               break;
           case 5:
               Debug.Log("<color=purple>"+message+"</color>");
               break;
       }
          lastDebugColor++;
          if (lastDebugColor > 5)
          {
                lastDebugColor = 0;
          }
   }
}
