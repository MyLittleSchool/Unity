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
        private float joystickDeg;

        //조이스틱
        public VariableJoystick joystick;

        public Vector3 stingDir;
        void Start()
        {
        }

        private void Update()
        {
            if (joystick.Vertical != 0 && joystick.Horizontal != 0)
            {
                joystickDeg = Mathf.Rad2Deg * Mathf.Atan2(joystick.Vertical, joystick.Horizontal);
            }

            // 찌르기 이모지 방향값 설정
            if (joystickDeg > -45 && joystickDeg < 45)
            {
                stingDir = transform.right;
            }
            else if (joystickDeg > 45 && joystickDeg < 135)
            {
                stingDir = transform.up;


            }
            else if (joystickDeg < -45 && joystickDeg > -135)
            {
                stingDir = -transform.up;


            }
            else if (joystickDeg > 135 || joystickDeg < -135)
            {
                stingDir = -transform.right;

            }
        }

        void FixedUpdate()
        {
            //PC
            OnMove(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
            //Mobile
            OnMove(joystick.Vertical, joystick.Horizontal);

        }



        private void OnMove(float vertical, float horizontal)
        {
            Vector3 playerDir = new Vector3(horizontal, vertical, 0);


            if (playerDir.magnitude > 1)
            {
                playerDir.Normalize();
            }
            transform.position += playerDir * playerSpeed * Time.fixedDeltaTime;



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
    }
}