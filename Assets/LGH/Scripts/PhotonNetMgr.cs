using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;

namespace GH
{
    public class PhotonNetMgr : MonoBehaviourPunCallbacks
    {
        // 포톤 닉네임
        public string nickName;

        //룸 이름
        public string roomName;
        void Start()
        {
            StartLogin();
        }
        void Update()
        {

        }
        public void StartLogin()
        {
            PhotonNetwork.GameVersion = "1.0.0";
            PhotonNetwork.NickName = nickName;
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnected()
        {
            base.OnConnected();

            print(MethodInfo.GetCurrentMethod().Name + " is call!");
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

            //나의 룸을 만든다.
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = 20;
            roomOpt.IsOpen = true;
            roomOpt.IsVisible = true;

            PhotonNetwork.CreateRoom(roomName, roomOpt, TypedLobby.Default);
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();

            //성공적으로 방이 만들어졌다.
            print(MethodInfo.GetCurrentMethod().Name + " is call!");
        }
    }
}