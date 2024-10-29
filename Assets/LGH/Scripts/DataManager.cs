using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GH
{

    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;
        public List<PhotonView> players = new List<PhotonView>();
        public GameObject player;
        public string playerName;
        public string playerSchool;
        public string playerCurrChannel;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
            }

        }


        void Start()
        {
        }

        void Update()
        {
          // PlayerFind();
        }

        private void PlayerFind()
        {

            if (player == null)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].IsMine)
                    {
                        player = players[i].gameObject;
                        print("플레이어 찾기 성공");
                    }
                }
            }
        }

   
      
    }

}