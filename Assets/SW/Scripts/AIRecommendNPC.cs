using MJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace SW
{
    public class AIRecommendNPC : Interactive
    {
        public GameObject chatBalloon;
        public TMP_Text chatBallonText;
        public string text;
        public int time = 5;
        private bool chatEnable;
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
        protected override void Start()
        {
            base.Start();
            chatBallonText.text = text;
        }
        public override void HighlightOn()
        {
            base.HighlightOn();
            ChatEnable = true;
        }
        public override void HighlightOff()
        {
            base.HighlightOff();
            if (ChatEnable)
            {
                ChatEnable = false;
            }
        }
        private void OnDestroy()
        {
            HighlightOff();
        }
        private IEnumerator SetChatBallon()
        {
            chatBalloon.SetActive(true);
            yield return new WaitForSeconds(time);
            chatBalloon.SetActive(false);
        }
        public override void Interact()
        {
            SceneUIManager.GetInstance().OnFriendsPanel();
            StartCoroutine(NextFrame());
        }
        IEnumerator NextFrame()
        {
            yield return null;
            WebSocketManager.GetInstance().friendsUI.ChangeTab(3);
        }
    }
}