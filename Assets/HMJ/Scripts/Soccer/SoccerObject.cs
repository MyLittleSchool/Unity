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
        }

        public void KickBall(Vector2 direction)
        {
            bounceObject.SetBallBounce(false);
            rigidbody.AddForce(direction * 1.0f, ForceMode2D.Impulse);
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    int layer = LayerMask.NameToLayer("NetCollision");
        //    if (collision.gameObject.layer == layer)
        //    {
        //        UIPanel.GetComponentInChildren<FadeOutUI>().FadeInOut(0.0f, 3.0f);
        //        StartCoroutine(ResetSoccerPosition(3.0f));
        //    }
        //}

        //private IEnumerator ResetSoccerPosition(float delayTime)
        //{
        //    yield return new WaitForSeconds(delayTime);

        //    transform.position = new Vector3(0.0f, 1.0f, 0.0f);
        //    rigidbody.velocity = Vector2.zero;
        //    rigidbody.angularVelocity = 0.0f;
        //    yield return null;
        //}
    }
}