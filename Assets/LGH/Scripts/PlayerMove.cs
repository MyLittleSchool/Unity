using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace GH
{

    public class PlayerMove : MonoBehaviour, IPunObservable
    {
        public float playerSpeed = 5f;
        public Animator playerAnimator;
        private bool moveAniTrriger = false;
        private float joystickDeg;

        //조이스틱
        public VariableJoystick joystick;

        public Vector3 stingDir;

        //포톤 변수값
        Vector3 myPos;
        public bool onIdle = true;

        public bool interactionMode = false;

        void Start()
        {
            joystick = GameManager.instance.Joystick;
            DataManager.instance.players.Add(gameObject.GetComponent<PhotonView>());
            stingDir = -transform.up;
            onIdle = true;
        }

        private void Update()
        {
            if (GetComponent<PhotonView>().IsMine)
            {

                if (joystick.Vertical != 0 && joystick.Horizontal != 0)
                {
                    joystickDeg = Mathf.Rad2Deg * Mathf.Atan2(joystick.Vertical, joystick.Horizontal);
                    onIdle = false;
                }
                else if (joystick.Vertical == 0 && joystick.Horizontal == 0)
                {
                    onIdle = true;
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
        }

        void FixedUpdate()
        {
            if (GetComponent<PhotonView>().IsMine)
            {

                //PC
                OnMove(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
                //Mobile
                OnMove(joystick.Vertical, joystick.Horizontal);
            }
            else
            {
                transform.position =  myPos;
            }
        }



        private void OnMove(float vertical, float horizontal)
        {


            Vector3 playerDir = new Vector3(horizontal, vertical, 0);


            if (playerDir.magnitude > 1)
            {
                playerDir.Normalize();
            }
            transform.position += playerDir * playerSpeed * Time.fixedDeltaTime;



        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // 만일 데이터를 서버에 전송(PhotonView.IsMine == true)하는 상태라면
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(stingDir);
                stream.SendNext(onIdle);

            }
            //그렇지 않고 만일 데이터를 서버로부터 읽어오는 상태라면
            else if (stream.IsReading)
            {
                //위에 받는 순서대로 변수를 캐스팅 해줘야 한다.
                myPos = (Vector3)stream.ReceiveNext();
                stingDir = (Vector3)stream.ReceiveNext();
                onIdle = (bool)stream.ReceiveNext();
            }
        }
    }
}