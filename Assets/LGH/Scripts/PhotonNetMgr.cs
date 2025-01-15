using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using MJ;
using SW;

namespace GH
{
    public class PhotonNetMgr : MonoBehaviourPunCallbacks
    {

        //상단 방 이름
        public TMP_Text topMenuText;

        // 포톤 닉네임
        private string playerName;

        //룸 이름
        public string roomName;

        // 방 리스트를 저장할 리스트
        private List<string> roomNames = new List<string>();

        public static PhotonNetMgr instance;

        public int sceneNum;

        //로딩 패널
        public GameObject loadingPanel;
        public Loading loading;

        public SceneUIManager sceneUIManager;

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
            playerName = AuthManager.GetInstance().userAuthData.userInfo.id.ToString();
            roomName = AuthManager.GetInstance().userAuthData.userInfo.id.ToString();
            StartLogin();

            //로딩채널 활성화
            loadingPanel.SetActive(true);
            loading = loadingPanel.GetComponent<Loading>();
        }
        void Update()
        {

        }
        public void StartLogin()
        {
            // 접속을 위한 설정
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.NickName = playerName;
            PhotonNetwork.AutomaticallySyncScene = false;
            // 접속을 서버에 요청
            PhotonNetwork.ConnectUsingSettings();
        }



        public override void OnConnected()
        {
            base.OnConnected();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            //  실패 원인 출력
            Debug.LogError("Disconnected from Server - " + cause);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");

            //서버의 로비로 들어간다
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            //서버 로비에 들어갔음을 알린다.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");

            //PhotonNetwork.JoinOrCreateRoom()
        }


        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
            // 이전 리스트를 지우고 업데이트
            roomNames.Clear();
            foreach (RoomInfo room in roomList)
            {
                if (!room.RemovedFromList)
                {
                    // 룸 이름 저장
                    roomNames.Add(room.Name);
                }
            }

            SuchRoom();
        }
        public void SuchRoom()
        {
            bool roomCheck = false;

            foreach (string roomN in roomNames)
            {
                if (roomN == roomName)
                {

                    JoinRoom();
                    roomCheck = true;
                }
            }

            if (!roomCheck)
            {
                CreateRoom();

            }
        }

        public void CreateRoom()
        {
            //나의 룸을 만든다.
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = 20;
            roomOpt.IsOpen = true;
            roomOpt.IsVisible = true;
            roomOpt.CleanupCacheOnLeave = false;
            PhotonNetwork.CreateRoom(roomName, roomOpt, TypedLobby.Default);
        }

        public void JoinRoom()
        {
            //방에 들어간다.
            PhotonNetwork.JoinRoom(roomName);

        }



        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();

            //성공적으로 방이 만들어졌다.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");


        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            JoinRoom();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            topMenuText.text = roomName;
            if (topMenuText.text == AuthManager.GetInstance().userAuthData.userInfo.id.ToString())
            {
                topMenuText.text = AuthManager.GetInstance().userAuthData.userInfo.nickname;
            }



            DataManager.instance.playerCurrChannel = roomName;
            PhotonChatMgr.instance.currChannel = roomName;
            PhotonChatMgr.instance.ChatChannelChange();

            // 성공적으로 방이 만들어졌다.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");
            GameObject joyStick = GameObject.Find("Variable Joystick");

            if (joyStick != null)
            {
                joyStick.GetComponent<Joystick>().ResetJoystick();
            }

            PlayerAnimation.GetInstance().SettingAvatar();
            sceneUIManager.MapTutorial();

        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            //룸 입장에 실패한 이유
            Debug.LogError(message);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
            //CreateRoom();
            //// 방에 입장한 친구들은 모두 1번 씬으로 이동하자
            //로딩창 활성화
            PhotonNetwork.LoadLevel(sceneNum);
            loadingPanel.SetActive(true);
            StartCoroutine(loading.SceneMove());
            GameManager.instance.CoSpwamPlayer();
            VoiceManager.GetInstance().MoveScene();

        }
        // 닉네임 변경 메서드
        //public void ChangeNickname(string newNickname)
        //{
        //    // 현재 로컬 플레이어의 닉네임 변경
        //    if (PhotonNetwork.IsMessageQueueRunning)
        //    {
        //        // 포톤 네트워크 내 플레이어 닉네임 설정
        //        PhotonNetwork.LocalPlayer.NickName = newNickname;

        //    }
        //}




    }

}