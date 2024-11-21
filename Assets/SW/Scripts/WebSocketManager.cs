using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
namespace SW
{
    public class WebSocketManager : MonoBehaviour
    {
        #region 싱글톤
        static WebSocketManager instance;
        public static WebSocketManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "WebSocketManager";
                go.AddComponent<WebSocketManager>();
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
        #endregion

        public string SERVER_ADDRESS { get; } = "ws://125.132.216.190:5544";
        public WebSocket webSocket;
        private void Start()
        {
            try
            {
                webSocket = new WebSocket(SERVER_ADDRESS + "/ws?userId=test1");
                webSocket.OnOpen += (sender, e) =>
                {
                    Debug.Log("서버와 연결되었습니다.");
                };
                webSocket.OnMessage += Receive;
                webSocket.OnClose += (sender, e) =>
                {
                    Debug.Log("서버와의 연결이 종료되었습니다.");
                };
                webSocket.OnError += (sender, e) =>
                {
                    Debug.LogError("WebSocket 오류: " + e.Message);
                };
                if (webSocket == null || !webSocket.IsAlive)
                    webSocket.Connect();
            }
            catch (Exception ex)
            {
                Debug.LogError("WebSocket 연결 중 오류 발생: " + ex.Message);
            }
        }
        private void Receive(object sender, MessageEventArgs e)
        {
            print(e.Data);
        }
        public void Send()
        {
            try
            {
                if (webSocket != null && webSocket.IsAlive)
                {

                    // 객체를 JSON 문자열로 변환
                    string jsonMessage = "test";

                    // WebSocket을 통해 서버로 전송
                    webSocket.Send(jsonMessage);

                    Debug.Log("보낸 메시지: " + jsonMessage);
                }
                else
                {
                    Debug.LogError("서버에 연결되지 않았습니다.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("메시지 전송 중 오류 발생: " + ex.Message);
            }
        }
        private void OnApplicationQuit()
        {
            DisconncectServer();
        }
        public void DisconncectServer()
        {
            try
            {
                if (webSocket == null)
                    return;

                if (webSocket.IsAlive)
                    webSocket.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Send();
            }
        }
    }
}