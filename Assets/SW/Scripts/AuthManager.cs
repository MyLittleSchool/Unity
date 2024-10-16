using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class AuthManager : MonoBehaviour
    {
        private static AuthManager instance;
        public static AuthManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "AuthManager";
                go.AddComponent<AuthManager>();
            }
            return instance;
        }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [System.Serializable]
        public struct AuthData
        {
            public int id;
            public string state;
            public string accessToken;
        }
        public AuthData UserAuthData { get; private set; }

        private void Start()
        {
            redirectUri = HttpManager.GetInstance().SERVER_ADRESS + "/login/oauth2/code/kakao";
            serverUrl = HttpManager.GetInstance().SERVER_ADRESS + "/kakaoLoginLog";
            OnKakaoLoginButtonClick();
        }

        // Kakao API 관련 설정
        private string kakaoLoginUrl = "https://kauth.kakao.com/oauth/authorize";
        private string clientId = "214a201e2f4c682aa352ec136a79189b";  // Kakao REST API Key
        private string redirectUri;  // Spring 서버의 리디렉션 URL

        // Spring 서버 URL (Authorization Code를 보낼 엔드포인트)
        private string serverUrl;

        private string clientState;


        // UI 버튼 클릭 시 실행되는 함수
        public void OnKakaoLoginButtonClick()
        {
            clientState = GenerateRandomClientId();
            //Debug.Log("Generated Client State: " + clientState);

            // 카카오 로그인 페이지로 이동하는 URL 생성
            string url = $"{kakaoLoginUrl}?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&state={clientState}&prompt=login";

            // 웹 브라우저에서 카카오 로그인 페이지 열기
            Application.OpenURL(url);

            // 이 단계에서 Spring 서버가 Authorization Code를 받게 되면 Unity에서 HTTP 요청을 보냅니다.
            StartCoroutine(getAccessToken());
        }

        // Spring 서버로 Authorization Code를 보내는 함수
        public IEnumerator getAccessToken()
        {
            string checkUrl = serverUrl + $"?state={clientState}";

            int cnt = 0;

            while (cnt < 500)
            {
                cnt++;
                UnityWebRequest www = UnityWebRequest.Get(checkUrl);
                yield return www.SendWebRequest();

                //Debug.Log("@@@");

                if (www.result == UnityWebRequest.Result.Success && www.downloadHandler.text.Length > 5)
                {
                    Debug.Log("Authorization Code successfully sent to server.");
                    Debug.Log(www.downloadHandler.text);
                    UserAuthData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
                    print(UserAuthData.id);
                    print(UserAuthData.state);
                    print(UserAuthData.accessToken);
                    break;
                }

                // 로그인 완료 신호가 올 때까지 대기 (1초 대기 후 재시도)
                yield return new WaitForSeconds(0.2f);
            }
        }

        // 랜덤 식별자 생성 함수 (UUID 형식으로 고유값 생성)
        public string GenerateRandomClientId()
        {
            return Guid.NewGuid().ToString();  // UUID 형식으로 고유값 생성
        }
    }
}
