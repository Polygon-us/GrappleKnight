using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCameraController2 : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float smoothTime = 0.4f;
    private float _negativeOrPositive;

    private Vector3 _targetOffset;
    private Vector3 _currentOffset;
    private Vector3 _mediumTargetOffset;
    private Vector2 targetVelocity = new Vector2(1,0);
    private Vector2 mediumVelocity = Vector2.zero;

    [SerializeField] private float _maxSpeed;
    [SerializeField] private CinemachineFramingTransposer transposer;
    [SerializeField] private CinemachineVirtualCamera _myCamera;

    private bool _OnMove;
    private float _curentTime;
    private void Awake()
    {
        transposer = _myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset = new Vector3(0,0,0);
    }


    private void Update()
    {
        SmoothCameraMovement();
    }

    private void SmoothCameraMovement()
    {
        if (transposer.m_TrackedObjectOffset != _targetOffset)
        {
            _curentTime += Time.deltaTime;
            float t = _curve.Evaluate(_curentTime);
            transposer.m_TrackedObjectOffset = Vector3.Lerp(_currentOffset, _targetOffset, t);
        }
        else
        {
            _curentTime = 0;
        }
    }

    public void MoveCameraPosition(float mover, int amount)
    {
        _currentOffset = transposer.m_TrackedObjectOffset;
        if (mover > 0)
        {
            _targetOffset.x = amount;
        }
        else if (mover < 0)
        {
            _targetOffset.x = -amount;
        }
    }
}
