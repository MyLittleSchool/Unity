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
        void Start()
        {
        }

        void Update()
        {
            OnMove();
        }

        void OnMove()
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
    }
}