using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCameraController : MonoBehaviour
{
    [SerializeField] private float valueToChange = 10;
    public void SetPosBySide(bool isRight)
    {
        Vector3 targetPos = transform.localPosition;
        targetPos.x =   valueToChange * (isRight ? 1 : -1);
        transform.localPosition = targetPos;
    }
}
