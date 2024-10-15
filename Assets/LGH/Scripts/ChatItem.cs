using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class ChatItem : MonoBehaviour
    {
        Text chatText;
        private void Awake()
        {
            chatText = GetComponent<Text>();

        }
        void Start()
        {
        }
    

        void Update()
        {

        }
        public void SetText(string msg)
        {
            chatText.text = msg;
        }
    }
}