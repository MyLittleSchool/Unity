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

            if (Input.GetKeyDown(KeyCode.Space) && bounceObject && bounceObject.GetBounceBall())
                KickBall(bounceObject.GetPlayerDirection());
            

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
        public float initialVelocityY = 5f;   // y방향 초기 속도
        public float gravity = -9.8f;         // 중력 가속도
        private float time = 0f;              // 시간 변수

        public void MoveBall()
        {
            if (!Move)
                return;
            // 시간에 따른 포물선 경로 계산
            time += Time.deltaTime;
            float x = initialVelocityX * time;
            float y = initialVelocityY * time + (0.5f * gravity * Mathf.Pow(time, 2));

            // 공의 위치 업데이트
            transform.position = new Vector2(firstPosition.x + x, firstPosition.y + y);
        }

        public void CheckPlayer()
        {
            if (bounceObject.CheckAroundPlayer(5.0f) && Input.GetKeyDown(KeyCode.K))
                bounceObject.StartBounce();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int layer = LayerMask.NameToLayer("NetCollision");
            if (collision.gameObject.name == "Player")
            {
                playerCollision();
            }
            else if (collision.gameObject.layer == layer)
            {
                UIPanel.GetComponentInChildren<FadeOutUI>().FadeInOut(0.0f, 3.0f);
                StartCoroutine(ResetSoccerPosition(3.0f));
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

            transform.position = new Vector3(0.0f, 1.0f, 0.0f);
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0.0f;
            Move = false;
            time = 0.0f;
            yield return null;
        }

        public void playerCollision()
        {
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");

            Vector2 kickDirection = new Vector2(horizontal, vertical).normalized;
            kickDirection.y += 0.05f;
            rigidbody.AddForce(kickDirection * 3.0f, ForceMode2D.Impulse);
        }
    }


}