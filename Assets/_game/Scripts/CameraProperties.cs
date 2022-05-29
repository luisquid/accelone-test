using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    public static Camera mainCam;
    public static float height;
    public static float width;

    private void Start()
    {
        mainCam = Camera.main;
        height = mainCam.orthographicSize;
        width = mainCam.orthographicSize * mainCam.aspect;
    }
}
