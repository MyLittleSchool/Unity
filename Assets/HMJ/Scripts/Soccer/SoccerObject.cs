using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MJ
{
    public class SoccerObject : MonoBehaviour
    {
        Rigidbody2D rigidbody;

        public GameObject UIPanel;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.drag = 0.5f;
        }
        private void Update()
        {

        }

        public void playerCollision()
        {
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");

            Vector2 kickDirection = new Vector2(horizontal, vertical).normalized;

            rigidbody.AddForce(kickDirection * 1.0f, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Player")
                playerCollision();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int layer = LayerMask.NameToLayer("NetCollision");
            if (collision.gameObject.layer == layer)
            {
                transform.position = new Vector3(0.0f, 1.0f, 0.0f);
                rigidbody.velocity = Vector2.zero;
                rigidbody.angularVelocity = 0.0f;
                UIPanel.GetComponentInChildren<FadeOutUI>().FadeInOut(0.0f, 5.0f);
            }
        }
    }
}