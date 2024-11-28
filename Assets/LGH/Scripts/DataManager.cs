using Photon.Pun;
using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static QuizCategory;

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
        public string privateRoomName;
        public QUIZCATEGORY curQuizChannel;
        public enum MapType { Login, School, MyClassroom, Square, Quiz, QuizSquare, ContestClassroom,Note, Others }
        public MapType mapType = MapType.MyClassroom;
        public MapType MapTypeState
        {
            get { return mapType; }
            set
            {
                mapType = value;
                // 통신
                UserPosInfo userPosInfo = new UserPosInfo();
                userPosInfo.type = "USER_POS_INFO";
                userPosInfo.userId = AuthManager.GetInstance().userAuthData.userInfo.id;
                userPosInfo.mapId = mapId;
                userPosInfo.mapType = mapType.ToString();
                userPosInfo.isOnline = true;
                WebSocketManager webSocketManager = WebSocketManager.GetInstance();
                webSocketManager.Send(webSocketManager.friendWebSocket, JsonUtility.ToJson(userPosInfo));
                // 여기서 bgm 변경

                SoundManager.instance.StopBgmSound();
                SoundManager.instance.PlayBgmSound(mapType);
            }
        }
        [Serializable]
        private struct UserPosInfo
        {
            public string type;
            public int userId;
            public int mapId;
            public string mapType;
            public bool isOnline;
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
            //Screen.SetResolution(360, 800, false);
            //Screen.fullScreen = false;
            SetFullScreenToMonitorResolution();
            privateRoomName = "";
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

        public void ChangeBGM()
        {

        }
        public void SetFullScreenToMonitorResolution()
        {
            // 현재 모니터의 기본 해상도로 설정
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;

            // 해상도를 모니터 해상도로 설정하고 전체화면 모드로 전환
            Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
        }
    }

}