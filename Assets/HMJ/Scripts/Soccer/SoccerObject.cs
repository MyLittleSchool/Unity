using GH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
// using static UnityEditor.PlayerSettings;

namespace MJ
{
    public class SoccerObject : MonoBehaviour, IPunObservable
    {
        public float forceMagnitude = 10.0f; // 힘의 세기
        public float verticalForce = 0.1f;   // y축 방향으로 추가적인 힘 (공이 뜨도록)


        Rigidbody2D rigidbody;

        public GameObject UIPanel;

        private BounceObject bounceObject;

        private bool Move = false;

        private PhotonView pv;
        // private Vector3 firstPosition;

        private float speed = 10.0f;

        //포톤 변수값
        Vector3 myPos;

        private PlayerMove playerMove;

        bool bPlayerCollision = false;
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.drag = 0.5f;

            bounceObject = GetComponentInChildren<BounceObject>();
            pv = GetComponent<PhotonView>();
        }


        private void Update()
        {
            ballUpdate();
        }

        private void FixedUpdate()
        {
            if (bPlayerCollision)
            {
                pv.RequestOwnership();
                rigidbody.velocity = Vector2.zero;
                rigidbody.angularVelocity = 0.0f;
                playerCollision();
                bPlayerCollision = false;
            }
            if (!pv.IsMine)
            {
                transform.position = myPos;
            }
        }

        public void KickBall(Vector2 direction)
        {
            bounceObject.SetBallBounce(false);
            Move = true;
            initialVelocityX = direction.x * 10.0f;
        }

        public float initialVelocityX = 20f;  // x방향 초기 속도
        public float initialVelocityY = 2f;   // y방향 초기 속도
        public float gravity = -9.8f;         // 중력 가속도
        public void MoveBall()
        {
            if (!Move || !pv.IsMine)
                return;
            // 현재 플레이어의 조이스틱을 가져와야한다.

            Vector2 kickDirection = new Vector2(GameManager.instance.Joystick.Horizontal, GameManager.instance.Joystick.Vertical).normalized;

            rigidbody.AddForce(kickDirection, ForceMode2D.Impulse);

            Move = false;
        }

        public void CheckPlayer()
        {
            bounceObject.StartBounce();
            pv.RequestOwnership();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            int layer = LayerMask.NameToLayer("NetCollision");
            //int layer1 = LayerMask.NameToLayer("OutCollision");
            if (collision.gameObject.name.Contains("Player") && (collision.gameObject.GetComponent<PhotonView>().IsMine) && !bPlayerCollision)
                bPlayerCollision = true;

            if (bounceObject.GetBounceBall())
                return;

            if (collision.gameObject.layer == layer)
            {
                //UIPanel.GetComponentInChildren<FadeOutUI>().FadeInOut(0.0f, 3.0f);
                StartCoroutine(ResetSoccerPosition(2.0f));
            }


        }


        private IEnumerator ResetSoccerPosition(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            transform.position = new Vector3(16.5f, -22.5f, 0.0f);
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0.0f;
            Move = false;
            yield return null;
        }

        public void playerCollision()
        {
            Vector2 joyStickData = new Vector2(GameManager.instance.Joystick.Horizontal, GameManager.instance.Joystick.Vertical).normalized;
            playerMove = DataManager.instance.player.GetComponent<PlayerMove>();

            KickBall(playerMove.joystickDeg, joyStickData);
        }

        public void KickBall(float degree, Vector2 joyStickData)
        {
            float radian = degree * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(radian) * Mathf.Abs(joyStickData.x), Mathf.Sin(radian) * Mathf.Abs(joyStickData.y + 0.05f));

            Vector2 force = direction * forceMagnitude;

            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public void DistanseCheck()
        {
            if(Vector3.Distance(new Vector3(16.5f, -22.5f, 0.0f), transform.position) > 10.0f)
                StartCoroutine(ResetSoccerPosition(0.5f));
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                myPos = transform.position;
            }
            else if (stream.IsReading)
            {              
                myPos = (Vector3)stream.ReceiveNext();
            }
        }

        public bool CheckPlayerDistance()
        {
            GameObject player = DataManager.instance.player;

            return player && Vector3.Distance(player.transform.position, transform.position) < 3.0f;
        }
        public void ballUpdate()
        {
            if (pv.IsMine)
            {
                if (bounceObject && bounceObject.GetBounceBall())
                    KickBall(bounceObject.GetPlayerDirection());
                else if (DataManager.instance.player && CheckPlayerDistance() && gameInteractButton.GetInstance().GetButtonDown())
                    CheckPlayer();
                MoveBall();
                DistanseCheck();
            }
        }
    }


}