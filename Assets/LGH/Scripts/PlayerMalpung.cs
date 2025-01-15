using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using SW;
using static UnityEngine.Rendering.DebugUI;
using static HttpManager;
using UnityEngine.Networking;

namespace GH
{


    public class PlayerMalpung : MonoBehaviourPun, IPunObservable
    {
        public UserInfoData getUserInfo;

        //말풍선
        public GameObject malpungPanel;
        public TMP_Text malpungText;
        public TMP_Text playerNameText;

        //말풍선 시간
        public float currtMalpungTime = 5.0f;
        private float maxMalpungTime = 5.0f;

        bool onMalpung = true;
        //bool onMalpung_Pun;

        [Header("입력말풍")]
        public GameObject inputMalpung;
        public TMP_InputField malpungInputField;


        void Start()
        {
            //말풍선 끄기
            malpungPanel.SetActive(false);

            // 말풍 텍스트 초기화
            malpungText.text = "";

            //PlayerNicknameSet();
            RPC_PlayerNicknameSet();
        }

        // Update is called once per frame
        void Update()
        {

            OnMalpung();
            malpungPanel.SetActive(onMalpung);
        }

        //말풍선 생기기
        private void OnMalpung()
        {
            currtMalpungTime += Time.deltaTime;
            if (currtMalpungTime < maxMalpungTime)
            {
                onMalpung = true;
            }
            else
            {
                //malpungPanel.SetActive(false);
                onMalpung = false;
            }
        }

        [PunRPC]
        public void MalPungText(string value)
        {
            malpungText.text = value;
            currtMalpungTime = 0;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // 만일 데이터를 서버에 전송(PhotonView.IsMine == true)하는 상태라면
            if (stream.IsWriting)
            {
            }
            //그렇지 않고 만일 데이터를 서버로부터 읽어오는 상태라면
            else if (stream.IsReading)
            {
            }
        }

        public void RPC_MalPungText(string value)
        {
            photonView.RPC(nameof(MalPungText), RpcTarget.All, value);
        }

        [PunRPC]
        public void PlayerNicknameSet()
        {
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/" + photonView.Owner.NickName;
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                string jsonData = "{ \"data\" : " + downloadHandler.text + "}";
                print(jsonData);
                //jsonData를 PostInfoArray 형으로 바꾸자.
                getUserInfo = JsonUtility.FromJson<UserInfoData>(jsonData);

                playerNameText.text = getUserInfo.data.nickname;
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        

        }

        public void RPC_PlayerNicknameSet()
        {
            photonView.RPC(nameof(PlayerNicknameSet), RpcTarget.All);

        }


    }

}
