using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class interestButton : MonoBehaviour
    {
        private TMP_Text interText;
        private TMP_Text buttonText;

        private bool onSelect = false;
        private Image image;
        private Button button;

        private LoginJoinUIManager loginJoinUIManager;
        void Start()
        {
            interText = GameObject.Find("interest_Text").GetComponent<TMP_Text>();
            buttonText = GetComponentInChildren<TMP_Text>();
            image = GetComponent<Image>();
            button = GetComponent<Button>();
            loginJoinUIManager = GameObject.Find("UIManager").GetComponentInChildren<LoginJoinUIManager>();
            button.onClick.AddListener(InterOnClick);
        }

        void Update()
        {
            if (onSelect)
            {
                image.color = new Color32(242, 136, 75, 255);
            }
            else
            {
                image.color = new Color32(242, 242, 242, 255);

            }

        }
        public void InterOnClick()
        {
            onSelect = onSelect ? false : true;

            if(onSelect)
            {
                //loginJoinUIManager.interestsQueue.Enqueue(buttonText.text);
            }
        }
    }

}