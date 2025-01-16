using GH;
using MJ;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
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
            PlayerPanel panel = PlayerList.instance.JoinReq();
            photonView.RPC(nameof(GetJoin), RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber, JsonUtility.ToJson(AuthManager.GetInstance().userAuthData.userInfo));
            StartCoroutine(Co_SetAvatar(AuthManager.GetInstance().userAuthData.userInfo.id, panel));
        }
        [PunRPC]
        public void GetJoin(int actorNumber, string _userInfo)
        {
            PlayerList.instance.GetJoin(actorNumber, ref playerUI);
            DataManager.instance.player.GetComponent<PhotonView>().RPC(nameof(JoinRes), PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), PhotonNetwork.LocalPlayer.ActorNumber, JsonUtility.ToJson(AuthManager.GetInstance().userAuthData.userInfo));
            playerPanel = playerUI.GetComponent<PlayerPanel>();
            playerPanel.userRPC = this;
            userInfo = JsonUtility.FromJson<UserInfo>(_userInfo);
            playerPanel.reportButton.onClick.AddListener(() =>
            {
                SceneUIManager.GetInstance().OnProfilePanel(userInfo);
            });
            StartCoroutine(Co_SetAvatar(playerPanel.userRPC.userInfo.id, playerPanel));
        }
        [PunRPC]
        public void JoinRes(int actorNumber, string _userInfo)
        {
            PlayerList.instance.JoinRes(actorNumber, ref playerUI);
            playerPanel = playerUI.GetComponent<PlayerPanel>();
            playerPanel.userRPC = this;
            userInfo = JsonUtility.FromJson<UserInfo>(_userInfo);
            playerPanel.reportButton.onClick.AddListener(() =>
            {
                SceneUIManager.GetInstance().OnProfilePanel(userInfo);
            });
            StartCoroutine(Co_SetAvatar(playerPanel.userRPC.userInfo.id, playerPanel));
        }
        IEnumerator Co_SetAvatar(int id, PlayerPanel panel)
        {
            yield return new WaitUntil(() => { return panel.gameObject.activeInHierarchy; });
            SetAvatar(id, panel);
        }
        public void SetAvatar(int id, PlayerPanel panel)
        {
            panel.userImage.AvatarGet(id);
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