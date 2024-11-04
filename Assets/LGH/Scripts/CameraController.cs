using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
namespace GH
{
    public class CameraController : MonoBehaviour
    {
        PixelPerfectCamera playerPixelCamera;
        int scrollSpeed = 100;
        Transform playerTransForm;

        enum TouchGesture
        {
            MOVE = 1,
            ZOOM
        }

        private void Start()
        {
            playerPixelCamera = GetComponent<PixelPerfectCamera>();
        }

        private void Update()
        {
            if (DataManager.instance != null)
            {
                playerTransForm = DataManager.instance.player.transform;

                //CameraZoomInOut();
                transform.position = new Vector3(playerTransForm.position.x, playerTransForm.position.y, transform.position.z);
            }
        }

        private void CameraZoomInOut()
        {
            float scrollWhell = Input.GetAxis("Mouse ScrollWheel");

            float scroll = scrollWhell * scrollSpeed;
            playerPixelCamera.assetsPPU += (int)scroll;
            playerPixelCamera.assetsPPU = Mathf.Clamp(playerPixelCamera.assetsPPU, 75, 150);
        }

        private void CameraDrage()
        {

        }

        private void MobileCameraDrage()
        {
            if (Input.touchCount == (int)TouchGesture.MOVE)
            {
                Touch touch = Input.touches[0];
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x - touch.deltaPosition.x,
                    Camera.main.transform.position.y - touch.deltaPosition.y,
                    Camera.main.transform.position.z);
            }
        }

        private void MobileCameraZoomInOut()
        {
            if (Input.touchCount == (int)TouchGesture.ZOOM)
            {
                Touch touch1 = Input.touches[0];
                Touch touch2 = Input.touches[1];


            }
        }
    }
}
