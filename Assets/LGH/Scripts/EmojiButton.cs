using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GH
{

    public class EmojiButton : MonoBehaviourPun
    {
        private int emojiIndex = 0;
        public string player = "Player";
        public PlayerEmoji playerEmoji;
        private Button but;
        //private PhotonView pv;
        void Start()
        {
           // pv = DataManager.instance.player.GetComponent<PhotonView>();
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

            playerEmoji.RPC_OnEmoji(emojiIndex);

        }
    }

}