using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{

    public class PlayerMove : MonoBehaviour
    {
        public float playerSpeed = 5f;
        void Start()
        {

        }

        void Update()
        {
            OnMove();
        }

        void OnMove()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Vector3 playerDir = new Vector3(h, v, 0);
            if(playerDir.magnitude > 1)
            {
                playerDir.Normalize();
            }
            transform.position += playerDir * playerSpeed * Time.deltaTime;
        }
    }
}