using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GH
{

    public class EmojiButton : MonoBehaviour
    {
        private int emojiIndex = 0;
        public string player = "Player";
        public PlayerEmoji playerEmoji;
        private Button but;
        void Start()
        {
           
            but = GetComponent<Button>();
            but.onClick.AddListener(EmojiPlay);
        }

        void Update()
        {

        }
        public void EmojiIndex(int idx)
        {
            emojiIndex = idx;
        }
        private void EmojiPlay()
        {
            playerEmoji.OnEmoji(emojiIndex);
        }
    }

}