using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class ChatItem : MonoBehaviour
    {
        TMP_Text chatText;
        private void Awake()
        {
            chatText = GetComponent<TMP_Text>();

        }
        void Start()
        {

        }

        void Update()
        {

        }
        public void SetText(string msg, Color chatColor)
        {
            string[] text = msg.Split(" ", 2);
            if (text[0] == DataManager.instance.playerName)
            {
                text[0] = "<color=#" + ColorUtility.ToHtmlStringRGB(chatColor) + ">" + text[0] + "</color>";
            }
            


            chatText.text = text[0] + " " + text[1];
        }
    }
}