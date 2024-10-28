using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace MJ
{
    public class UIManager : MonoBehaviour
    {
        #region Button
        [Header("로그인 버튼")]
        public UnityEngine.UI.Button loginButton;
        public UnityEngine.UI.Button KakaoLoginButton;

        [Header("회원가입 버튼")]
        public UnityEngine.UI.Button joinButton;
        public UnityEngine.UI.Button kakaoJoinButton;

        [Header("닫기 버튼")]

        public UnityEngine.UI.Button kakaoLoginCloseButton;
        public UnityEngine.UI.Button kakaoJoinCloseButton;
        #endregion

        #region Panel
        [Header("로그인 패널")]
        public GameObject loginPanel;
        public GameObject KakaoLoginPanel;

        [Header("회원가입 패널")]
        public GameObject JoinPanel;
        public GameObject KakaoJoinPanel;
        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginPanel);
            joinButton.onClick.AddListener(OnJoinPanel);

            KakaoLoginButton.onClick.AddListener(OnKakaoLoginPanel);
            kakaoJoinButton.onClick.AddListener(OnKakaoJoinPanel);

            kakaoLoginCloseButton.onClick.AddListener(OnKakaoLoginClosePanel);
            kakaoJoinCloseButton.onClick.AddListener(OnKakaoJoinClosePanel);

        }

        public void OnLoginPanel()
        {
            loginPanel.SetActive(true);
            JoinPanel.SetActive(false);
        }

        public void OnJoinPanel()
        {
            loginPanel.SetActive(false);
            JoinPanel.SetActive(true);
        }

        public void OnKakaoLoginPanel()
        {
            KakaoLoginPanel.SetActive(true);
            KakaoJoinPanel.SetActive(false);
        }

        public void OnKakaoJoinPanel()
        {
            KakaoLoginPanel.SetActive(false);
            KakaoJoinPanel.SetActive(true);
        }

        public void OnKakaoJoinClosePanel()
        {
            KakaoJoinPanel.SetActive(false);
        }

        public void OnKakaoLoginClosePanel()
        {
            KakaoLoginPanel.SetActive(false);
        }
        
    }
}