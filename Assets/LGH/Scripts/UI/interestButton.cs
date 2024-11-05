using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class interestButton : MonoBehaviour
    {
        private TMP_Text buttonText;

        private Button button;

        private LoginJoinUIManager loginJoinUIManager;
        void Start()
        {
            buttonText = GetComponentInChildren<TMP_Text>();
            button = GetComponent<Button>();
            loginJoinUIManager = GameObject.Find("UIManager").GetComponentInChildren<LoginJoinUIManager>();
            button.onClick.AddListener(InterOnClick);
        }

        void Update()
        {
           

        }
        public void InterOnClick()
        {
            loginJoinUIManager.InterestSlect(buttonText.text, GetComponent<Image>());
        }
    }

}