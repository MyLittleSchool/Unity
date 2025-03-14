using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GH
{
    public class StingMove : MonoBehaviour
    {
        private SpriteRenderer stingSpriteRenderer;
        private float stringSpeed = 3;
        private float stringAlpha = 2f;
        public string stingPlayer;
        void Start()
        {
            stingSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Update()
        {
            OnSting();
        }
        private void OnSting()
        {
            transform.position += transform.right * stringSpeed * Time.deltaTime;
            stingSpriteRenderer.color -= new Color(0, 0, 0, 1) * stringAlpha * Time.deltaTime;

            Destroy(transform.gameObject, 0.5f);
        }
    }
}