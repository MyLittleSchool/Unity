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
        public int mapId;
        public enum MapType { Login, School, MyClassroom, Square, Quiz, QuizSquare, ContestClassroom,Note, Others }
        public MapType mapType = MapType.MyClassroom;
        public MapType MapTypeState
        {
            get { return mapType; }
            set
            {
                mapType = value;
                // 통신

            }
        }
        public GameObject setTileObj;
        public int setTileObjId;



        [Header("6. 관심사 리스트")]
        public List<string> interests;
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
            Screen.SetResolution(360, 800, false);
            Screen.fullScreen = false;

        }
        public void PlayerFind()
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