using GH;
using MJ;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
namespace SW
{
    public class UserRPC : MonoBehaviourPunCallbacks
    {
        private PhotonView pv;
        private int actorNumber;
        public GameObject playerUI;
        public PlayerPanel playerPanel;
        public UserInfo userInfo;
        private void Start()
        {
            pv = GetComponent<PhotonView>();
            actorNumber = pv.OwnerActorNr;
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
            PlayerList.instance.Leave();
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
            DataManager.instance.player.GetComponent<PhotonView>().RPC(nameof(JoinRes), PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), PhotonNetwork.LocalPlayer.ActorNumber, JsonUtility.ToJson(AuthManager.GetInstance().userAuthData.userInfo));
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

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            if (PhotonNetwork.IsMasterClient && actorNumber == otherPlayer.ActorNumber)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}