using GH;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
namespace SW
{
    public class AIChatBotNPC : Interactive
    {
        public GameObject chatBalloon;
        public TMP_Text chatBallonText;
        public int time = 5;
        private bool chatEnable;
        private bool isInteracting;
        public bool IsInteracting
        {
            get { return isInteracting; }
            set
            {
                isInteracting = value;
                if (value)
                {
                    PhotonChatMgr.instance.ChatModeState = PhotonChatMgr.ChatMode.AIChatBot;
                    PhotonChatMgr.instance.NPC = this;
                    PhotonChatMgr.instance.OnChatInput();
                }
                else
                {
                    PhotonChatMgr.instance.ChatModeState = PhotonChatMgr.ChatMode.Default;
                }
            }
        }
        public bool ChatEnable
        {
            get { return chatEnable; }
            set
            {
                chatEnable = value;
                if (chatEnable)
                {
                    StopAllCoroutines();
                    StartCoroutine(SetChatBallon());
                }
                else
                {
                    StopAllCoroutines();
                    chatBalloon.SetActive(false);
                }
            }
        }
        public void ReqChat(string text)
        {
            ChatReqInfo chatReqInfo = new ChatReqInfo();
            chatReqInfo.user_type = "ÁßÇÐ»ý";
            chatReqInfo.message = text;
            chatReqInfo.userId = AuthManager.GetInstance().userAuthData.userInfo.id;
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/chat-bot-log";
            info.body = JsonUtility.ToJson(chatReqInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                ChatResInfo chatResInfo = JsonUtility.FromJson<ChatResInfo>(res.text);
                chatBallonText.text = chatResInfo.aiMessage;
                ChatEnable = true;
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
        private struct ChatReqInfo
        {
            public string user_type;
            public string message;
            public int userId;
        }
        private struct ChatResInfo
        {
            public string aiMessage;
        }

        public override void Interact()
        {
            IsInteracting = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void HighlightOn()
        {
            base.HighlightOn();
        }
        public override void HighlightOff()
        {
            base.HighlightOff();
            if (IsInteracting) IsInteracting = false;
        }
        private void OnDestroy()
        {
            if (IsInteracting) IsInteracting = false;
            HighlightOff();
        }
        private IEnumerator SetChatBallon()
        {
            chatBalloon.SetActive(true);
            yield return new WaitForSeconds(time);
            chatBalloon.SetActive(false);
        }
    }
}