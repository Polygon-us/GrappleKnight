using Cinemachine;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    public static CinemachineController Instance;
    
    private float _initialIntensity;
    private float _timeMovement;
    private float _totalTimeMovement;
    
    private CinemachineVirtualCamera _myCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineShake;
    
    void Awake()
    {
        Instance = this;
        _myCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineShake = _myCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    
    void Update()
    {
        Execute();
    }

   public void MoveCamera(float intensity, float frecuency, float time)
    {
        _cinemachineShake.m_AmplitudeGain = intensity;
        _cinemachineShake.m_FrequencyGain = frecuency;

        _initialIntensity = intensity;
        _totalTimeMovement = time;
        _timeMovement = time;
    }

    public void Execute()
    {
        if(_timeMovement > 0)
        {
            _timeMovement -= Time.deltaTime;
            _cinemachineShake.m_AmplitudeGain = Mathf.Lerp(_initialIntensity,0, 1 -(_timeMovement/_totalTimeMovement));
        }
    }
}
