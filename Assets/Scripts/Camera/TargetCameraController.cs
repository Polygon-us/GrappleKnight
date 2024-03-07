using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetCameraController : MonoBehaviour
{
    public static TargetCameraController instance;

    [SerializeField] private float smoothness = 10f;
    [SerializeField] private float smoothTime = 0.01f; 
    private float _negativeOrPositive;

    private Vector2 _targetOffset;
    private Vector2 _mediumTargetOffset;

    [SerializeField] private CinemachineFramingTransposer transposer;
    [SerializeField] private CinemachineVirtualCamera _myCamera;

    private void Awake()
    {
        instance = this;
        transposer = _myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void MoveCameraPosition(float mover)
    {
        float targetX, mediumX;

        if (mover > 0)
        {
            targetX = 8f;
            mediumX = 6f;
        }
        else if (mover < 0)
        {
            targetX = -6f;
            mediumX = -4f;
        }
        else
        {
            return;
        }

        _targetOffset = new Vector2(targetX, 0);
        _mediumTargetOffset = new Vector2(mediumX, 0);

        Vector2 velocity = Vector2.zero;
        transposer.m_TrackedObjectOffset = Vector2.SmoothDamp(
            transposer.m_TrackedObjectOffset, _targetOffset, ref velocity, smoothTime);

        transposer.m_TrackedObjectOffset = Vector2.Lerp(
            transposer.m_TrackedObjectOffset, _mediumTargetOffset, Time.deltaTime * smoothness);
    }   
}


 

    

    



