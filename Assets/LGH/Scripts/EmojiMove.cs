using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GH
{
    public class EmojiMove : MonoBehaviour
    {
        private float emojiWaitTime = 1f;
        private float emojiCurrTime = 0;
        private float emojiUpSpeed = 2;
        private SpriteRenderer emojiSpriteRenderer;
        void Start()
        {
            emojiSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Update()
        {
            EmojiUp();
        }
        private void EmojiUp()
        {
            emojiCurrTime += Time.deltaTime;
            if (emojiCurrTime > emojiWaitTime)
            {
                transform.position += transform.up * emojiUpSpeed * Time.deltaTime;
                emojiSpriteRenderer.color -= new Color(0, 0, 0, 1) * emojiUpSpeed * Time.deltaTime;
            }
            Destroy(gameObject, 2.5f);
        }
    }
}
