using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Transform camera;

    [SerializeField]
    private Vector3 positionStrength;

    [SerializeField]
    private Vector3 rotationStrength;

    private static event Action Shake;
    public static void Invoke()
    {
        Shake?.Invoke();
    }

    private void OnEnable() => Shake += CameraShaking;
    private void OnDisable() => Shake -= CameraShaking;
    private void CameraShaking()
    {
        camera.DOComplete();
        camera.DOShakePosition(0.3f, positionStrength);
        camera.DOShakePosition(0.3f, rotationStrength);
    }
}
