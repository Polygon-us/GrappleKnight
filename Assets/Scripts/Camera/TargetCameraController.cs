using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetCameraController : MonoBehaviour
{
    public static TargetCameraController instance;

    [SerializeField] private float smoothTime = 0.4f; 
    private float _negativeOrPositive;

    private Vector2 _targetOffset;
    private Vector2 _mediumTargetOffset;
    private Vector2 targetVelocity = Vector2.zero;
    private Vector2 mediumVelocity = Vector2.zero;

    [SerializeField] private CinemachineFramingTransposer transposer;
    [SerializeField] private CinemachineVirtualCamera _myCamera;

    private void Awake()
    {

        instance = this;
        transposer = _myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

 
    public void MoveCameraPosition(float mover, float maxSpeed = 1f)
    {
        Debug.Log("Hello");
        float targetX, mediumX;

        if (mover > 0)
        {
            targetX = 8f;
            mediumX = 6f;
        }
        else if (mover < 0)
        {
            targetX = -12f;
            mediumX = -4f;
        }
        else
        {
            return;
        }

        _targetOffset = new Vector2(targetX, 0);
        _mediumTargetOffset = new Vector2(mediumX, 0);

        float deltaTime = Time.deltaTime; 

        transposer.m_TrackedObjectOffset = Vector2.SmoothDamp(
            transposer.m_TrackedObjectOffset, _targetOffset, ref targetVelocity, smoothTime, maxSpeed, deltaTime);

        //transposer.m_TrackedObjectOffset = Vector2.SmoothDamp(
        //    transposer.m_TrackedObjectOffset, _mediumTargetOffset, ref mediumVelocity, smoothTime, maxSpeed, deltaTime);
    }
}


 

    

    



