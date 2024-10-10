using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
namespace GH
{
    public class CameraController : MonoBehaviour
    {
        PixelPerfectCamera playerPixelCamera;
        int scrollSpeed = 100;
        void Start()
        {
            playerPixelCamera = GetComponent<PixelPerfectCamera>();
        }

        void Update()
        {
            CameraZoomInOut();
        }

        void CameraZoomInOut()
        {
            float scrollWhell = Input.GetAxis("Mouse ScrollWheel");

            float scroll = scrollWhell * scrollSpeed;
            playerPixelCamera.assetsPPU += (int)scroll;
            
            if(playerPixelCamera.assetsPPU < 75)
            {
                playerPixelCamera.assetsPPU = 75;
            }
            else if(playerPixelCamera.assetsPPU > 150)
            {
                playerPixelCamera.assetsPPU = 150;
            }
        }
    }
}
