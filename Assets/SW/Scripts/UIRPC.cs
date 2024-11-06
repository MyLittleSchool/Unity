using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace SW
{
    public class UIRPC : MonoBehaviourPun
    {
        private PhotonView pv;
        public GameObject playerUI;
        private void Start()
        {
            pv = GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                PlayerList.instance.ResetContents();
                JoinReq();
            }
        }
        private void OnDestroy()
        {
            Destroy(playerUI);
        }
        public void JoinReq()
        {
            PlayerList.instance.JoinReq();
            photonView.RPC(nameof(GetJoin), RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        [PunRPC]
        public void GetJoin(int actorNumber)
        {
            PlayerList.instance.GetJoin(actorNumber, ref playerUI);
            photonView.RPC(nameof(JoinRes), PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), PhotonNetwork.LocalPlayer.ActorNumber);
        }
        [PunRPC]
        public void JoinRes(int actorNumber)
        {
            PlayerList.instance.JoinRes(actorNumber);
        }
    }
}