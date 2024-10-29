using GH;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace MJ
{
    public class SoccerObject : MonoBehaviour
    {
        Rigidbody2D rigidbody;

        public GameObject UIPanel;

        private BounceObject bounceObject;

        bool Move = false;

        Vector3 firstPosition;
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.drag = 0.5f;

            bounceObject = GetComponentInChildren<BounceObject>();
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.K) && bounceObject && bounceObject.GetBounceBall())
                KickBall(bounceObject.GetPlayerDirection());
            

            if(DataManager.instance.player)
                CheckPlayer();
            MoveBall();
        }

        public void KickBall(Vector2 direction)
        {
            bounceObject.SetBallBounce(false);
            Move = true;
            initialVelocityX = direction.x * 10.0f;
            firstPosition = transform.position;
            // rigidbody.AddForce(direction * 1.0f, ForceMode2D.Impulse);
        }

        public float initialVelocityX = 20f;  // x방향 초기 속도
        public float initialVelocityY = 2f;   // y방향 초기 속도
        public float gravity = -9.8f;         // 중력 가속도
        private float timeX = 0f;              // 시간 변수
        private float timeY = 0f;              // 시간 변수
        public void MoveBall()
        {
            if (!Move)
                return;
            Vector2 kickDirection = new Vector2(GameManager.instance.Joystick.Horizontal, GameManager.instance.Joystick.Vertical).normalized;
            rigidbody.AddForce(kickDirection, ForceMode2D.Impulse);

            Move = false;
        }

        public void CheckPlayer()
        {
            if (bounceObject.CheckAroundPlayer(2.0f) && !bounceObject.GetBounceBall() && !Move && Input.GetKeyDown(KeyCode.K))
                bounceObject.StartBounce();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            int layer = LayerMask.NameToLayer("NetCollision");
            int layer1 = LayerMask.NameToLayer("OutCollision");
            if (collision.gameObject.name.Contains("Player"))
                playerCollision();

            if (bounceObject.GetBounceBall())
                return;
            if (collision.gameObject.layer == layer)
            {
                UIPanel.GetComponentInChildren<FadeOutUI>().FadeInOut(0.0f, 3.0f);
                StartCoroutine(ResetSoccerPosition(2.0f));
            }
            else if (collision.gameObject.layer == layer1)
            {
                StartCoroutine(ResetSoccerPosition(0.5f));
            }


        }
        private void OnCollisionEnter2D(Collision2D collision)
        {

        }

        private void OnCollisionStay2D(Collision2D collision)
        {

        }

        private IEnumerator ResetSoccerPosition(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            transform.position = new Vector3(31.5f, 0.5f, 0.0f);
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0.0f;
            Move = false;
            timeX = 0.0f;
            timeY = 0.0f;
            yield return null;
        }

        public void playerCollision()
        {
            Vector2 kickDirection = new Vector2(GameManager.instance.Joystick.Horizontal, GameManager.instance.Joystick.Vertical).normalized;
            kickDirection.y += 0.05f;
            rigidbody.AddForce(kickDirection * 5.0f, ForceMode2D.Impulse);
        }
    }


}