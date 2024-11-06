using GH;
using MJ;
using Photon.Pun;
using UnityEngine;
namespace SW
{
    public class UserRPC : MonoBehaviourPun
    {
        private PhotonView pv;
        public GameObject playerUI;
        public PlayerPanel playerPanel;
        public UserInfo userInfo;
        private void Awake()
        {
            DataManager.instance.mapId = AuthManager.GetInstance().userAuthData.userInfo.id;
        }
        private void Start()
        {
            pv = GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                if (PlayerList.instance == null)
                {
                    PlayerList.instance = SceneUIManager.GetInstance().playerList.GetComponent<PlayerList>();
                }
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
            photonView.RPC(nameof(GetJoin), RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber, JsonUtility.ToJson(AuthManager.GetInstance().userAuthData.userInfo));
        }
        [PunRPC]
        public void GetJoin(int actorNumber, string _userInfo)
        {
            PlayerList.instance.GetJoin(actorNumber, ref playerUI);
            photonView.RPC(nameof(JoinRes), PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), PhotonNetwork.LocalPlayer.ActorNumber, JsonUtility.ToJson(AuthManager.GetInstance().userAuthData.userInfo));
            playerPanel = playerUI.GetComponent<PlayerPanel>();
            playerPanel.userRPC = this;
            userInfo = JsonUtility.FromJson<UserInfo>(_userInfo);
        }
        [PunRPC]
        public void JoinRes(int actorNumber, string _userInfo)
        {
            PlayerList.instance.JoinRes(actorNumber, ref playerUI);
            playerPanel = playerUI.GetComponent<PlayerPanel>();
            playerPanel.userRPC = this;
            userInfo = JsonUtility.FromJson<UserInfo>(_userInfo);
        }
    }
}