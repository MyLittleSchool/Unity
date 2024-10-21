using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{

    public class PlayerMove : MonoBehaviour
    {
        public float playerSpeed = 5f;
        public Animator playerAnimator;
        private bool moveAniTrriger = false;

        //Á¶ÀÌ½ºÆ½
        public VariableJoystick joystick;

        void Start()
        {
        }

        void Update()
        {
            OnPCMove();
            OnMobileMove();
        }

        void OnPCMove()
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            Vector3 playerDir = new Vector3(h, v, 0);

            
            if (playerDir.magnitude > 1)
            {
                playerDir.Normalize();
            }
            transform.position += playerDir * playerSpeed * Time.deltaTime;



            if (h == 0 && v == 0 && moveAniTrriger)
            {
                playerAnimator.SetTrigger("Idle");
                moveAniTrriger = false;
            }
            if((h != 0 || v != 0) && !moveAniTrriger)
            {
                playerAnimator.SetTrigger("Run");
                moveAniTrriger = true ;

            }

        }

        void OnMobileMove()
        {
            float v = joystick.Vertical;
            float h = joystick.Horizontal;

            Vector3 playerDir = new Vector3(h, v, 0);


            if (playerDir.magnitude > 1)
            {
                playerDir.Normalize();
            }
            transform.position += playerDir * playerSpeed * Time.deltaTime;



            if (h == 0 && v == 0 && moveAniTrriger)
            {
                playerAnimator.SetTrigger("Idle");
                moveAniTrriger = false;
            }
            if ((h != 0 || v != 0) && !moveAniTrriger)
            {
                playerAnimator.SetTrigger("Run");
                moveAniTrriger = true;

            }

        }
    }
}