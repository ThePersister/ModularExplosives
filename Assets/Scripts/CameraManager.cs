using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private Transform _cameraRoot;

    public Transform CameraRoot
    {
        get
        {
            return _cameraRoot;
        }
    }

    private void Start()
    {
        _cameraRoot = transform;
    }
}
