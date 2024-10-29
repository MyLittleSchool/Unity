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
                DataManager.instance.playerCurrChannel = "이규현";
                PhotonNetMgr.instance.roomName = "이규현";

                PhotonNetwork.LeaveRoom();
                PhotonNetMgr.instance.sceneNum = 2;
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