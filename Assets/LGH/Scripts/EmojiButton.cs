using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GH
{

    public class EmojiButton : MonoBehaviour
    {
        private int emojiIndex = 0;
        public string player = "DumPlayer";
        private PlayerEmoji playerEmoji;
        private Button but;
        void Start()
        {
            playerEmoji = GameObject.Find(player).GetComponent<PlayerEmoji>();
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