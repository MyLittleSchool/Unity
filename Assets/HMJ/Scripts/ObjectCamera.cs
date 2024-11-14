using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // 기준 화면 비율 (16:9)
    public float orthographicSize = 5f; // 기준 orthographic size

    public Camera cam;

    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        // 현재 화면 비율
        float currentAspect = (float)Screen.width / Screen.height;

        // 화면 비율에 따라 orthographic size 조정
        if (currentAspect >= targetAspect)
        {
            cam.orthographicSize = orthographicSize;
        }
        else
        {
            float scale = targetAspect / currentAspect;
            cam.orthographicSize = orthographicSize * scale;
        }
    }

    void Update()
    {
        // 화면이 변경되면 다시 조정
        if (Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
        {
            AdjustCameraSize();
        }
    }
}
