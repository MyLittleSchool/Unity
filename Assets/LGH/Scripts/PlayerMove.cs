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
        //해상도
        //Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        //print(pos);
        //rt.anchoredPosition = pos;
        //print(Screen.width + ", " + Screen.height);

        //조이스틱
        public VariableJoystick joystick;

        private float vertical;
        private float horizontal;

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
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");

            Vector3 playerDir = new Vector3(horizontal, vertical, 0);


            if (playerDir.magnitude > 1)
            {
                playerDir.Normalize();
            }
            transform.position += playerDir * playerSpeed * Time.deltaTime;



            if (horizontal == 0 && vertical == 0 && moveAniTrriger)
            {
                playerAnimator.SetTrigger("Idle");
                moveAniTrriger = false;
            }
            if ((horizontal != 0 || vertical != 0) && !moveAniTrriger)
            {
                playerAnimator.SetTrigger("Run");
                moveAniTrriger = true;

            }

        }

        void OnMobileMove()
        {
            vertical = joystick.Vertical;
            horizontal = joystick.Horizontal;

            Vector3 playerDir = new Vector3(horizontal, vertical, 0);


            if (playerDir.magnitude > 1)
            {
                playerDir.Normalize();
            }
            transform.position += playerDir * playerSpeed * Time.deltaTime;



            if (horizontal == 0 && vertical == 0 && moveAniTrriger)
            {
                playerAnimator.SetTrigger("Idle");
                moveAniTrriger = false;
            }
            if ((horizontal != 0 || vertical != 0) && !moveAniTrriger)
            {
                playerAnimator.SetTrigger("Run");
                moveAniTrriger = true;

            }

        }

        //public void StingDir()
        //{
        //    Vector3 dir;

        //    return dir
        //}
    }
}