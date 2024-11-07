using MJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GH
{

    public class interestButton : MonoBehaviour
    {
        private TMP_Text buttonText;

        private Button button;

        private LoginJoinUIManager loginJoinUIManager;
        private SceneUIManager sceneUIManager;

        void Start()
        {
            buttonText = GetComponentInChildren<TMP_Text>();
            button = GetComponent<Button>();

            loginJoinUIManager = GameObject.Find("UIManager").GetComponentInChildren<LoginJoinUIManager>();

            sceneUIManager = GameObject.Find("UIManager").GetComponentInChildren<SceneUIManager>();


            button.onClick.AddListener(InterOnClick);
        }

        void Update()
        {


        }
        public void InterOnClick()
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
            loginJoinUIManager.InterestSlect(buttonText.text, GetComponent<Image>());
            }
            else
            {
                sceneUIManager.InterestSlect(buttonText.text, GetComponent<Image>());

            }
        }
    }

}