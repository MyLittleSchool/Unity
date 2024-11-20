using System.Collections;
using TMPro;
using UnityEngine;
namespace SW
{
    public class ChatBulloonNPC : Interactive
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
        public override void Interact()
        {
            ChatEnable = true;
        }
        protected override void Start()
        {
            base.Start();
            chatBallonText.text = text;
        }
        public override void HighlightOn()
        {
            base.HighlightOn();
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
    }
}