using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{

    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;
        public List<PhotonView> players = new List<PhotonView>();
        public GameObject player;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public VariableJoystick Joystick;
        public Transform emojiTransform;

        void Start()
        {

        }

        void Update()
        {
            if(player == null)
            {
                for(int i = 0; i< players.Count; i++)
                {
                    if (players[i].IsMine)
                    {
                        player = players[i].gameObject;
                        print("플레이어 찾기 성공");
                    }
                }
            }

        }

        public void OnSting()
        {
            PlayerEmoji playerEmoji = player.GetComponent<PlayerEmoji>();
            playerEmoji.OnString();
        }
    }

}