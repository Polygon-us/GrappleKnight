using Cinemachine;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    [Header("Cam Parameters")]
    private float _initialIntensity;
    private float _timeMovement;
    private float _totalTimeMovement;

    [Header("References")]
    private CinemachineVirtualCamera _myCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineShake;
    
    public static CinemachineController Instance;

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
