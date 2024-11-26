using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using SW;

namespace GH
{


    public class PlayerMalpung : MonoBehaviourPun, IPunObservable
    {
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
            playerNameText.text = AuthManager.GetInstance().userAuthData.userInfo.name;
        }

        // Update is called once per frame
        void Update()
        {

            OnMalpung();
            malpungPanel.SetActive(onMalpung);
        }
        public void PlayerNameSet()
        {
            playerNameText.text = AuthManager.GetInstance().userAuthData.userInfo.name;
            PhotonNetMgr.instance.ChangeNickname(AuthManager.GetInstance().userAuthData.userInfo.name);

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




    }

}
