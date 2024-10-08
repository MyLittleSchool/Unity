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
        private void Start()
        {
            LogInReq();

            //HttpManager.HttpInfo httpInfo = new HttpManager.HttpInfo();
            //httpInfo.url = "http://125.132.216.190:5544/login";
            //httpInfo.onComplete = (DownloadHandler response) => { print("¼º°ø"); };
            //StartCoroutine(HttpManager.GetInstance().Get(httpInfo));
        }
        void LogInReq()
        {
            print("test");
            string clientId = "214a201e2f4c682aa352ec136a79189b";
            string redirect_URI = "http://125.132.216.190:5544/";
            string kakaoLoginUrl = "https://kauth.kakao.com/oauth/authorize" +
                               "?client_id=" + clientId +
                               "&redirect_uri=" + redirect_URI +
                               "&response_type=code";
            Application.OpenURL(kakaoLoginUrl);
        }
        string sessionId = System.Guid.NewGuid().ToString();
    }
}
