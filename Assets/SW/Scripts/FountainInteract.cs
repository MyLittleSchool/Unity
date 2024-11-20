using GH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SW
{
    public class FountainInteract : Interactive
    {

        protected override void Start()
        {
            base.Start();

        }
        public override void Interact()
        {
            if (gameObject.name == "GoClass")
            {
                DataManager.instance.playerCurrChannel = DataManager.instance.playerName;
                PhotonNetMgr.instance.roomName = DataManager.instance.playerName;
                DataManager.instance.mapId = AuthManager.GetInstance().userAuthData.userInfo.id;
                DataManager.instance.MapTypeState = DataManager.MapType.MyClassroom;

                //DataManager.instance.player = null;
                PhotonNetwork.LeaveRoom();
                PhotonNetMgr.instance.sceneNum = 2;
                //SceneManager.LoadScene(2);
                //PhotonNetMgr.instance.CreateRoom();
            }
            else
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    SceneMgr.instance.SquareIn();

                }
                else if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    SceneMgr.instance.SchoolIn();

                }

            }
            print("분수상호작용");
        }
    }
}