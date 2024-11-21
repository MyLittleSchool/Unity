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
        public WebSocket friendWebSocket;
        public FriendsUI friendsUI;
        public void LogIn(int id)
        {
            try
            {
                friendWebSocket = new WebSocket(SERVER_ADDRESS + "/ws?userId=" + id);
                friendWebSocket.OnOpen += (sender, e) =>
                {
                    Debug.Log("서버와 연결되었습니다.");
                };
                friendWebSocket.OnMessage += Receive;
                friendWebSocket.OnClose += (sender, e) =>
                {
                    Debug.Log("서버와의 연결이 종료되었습니다.");
                };
                friendWebSocket.OnError += (sender, e) =>
                {
                    Debug.LogError("WebSocket 오류: " + e.Message);
                };
                if (friendWebSocket == null || !friendWebSocket.IsAlive)
                    friendWebSocket.Connect();
            }
            catch (Exception ex)
            {
                Debug.LogError("WebSocket 연결 중 오류 발생: " + ex.Message);
            }
        }
        private Queue<string> receiveQueue = new Queue<string>();
        private void Receive(object sender, MessageEventArgs e)
        {
            receiveQueue.Enqueue(e.Data);
        }
        private void Update()
        {
            while (receiveQueue.Count != 0)
            {
                string data = receiveQueue.Dequeue();
                print(data);
                GetReceiveType type = JsonUtility.FromJson<GetReceiveType>(data);
                if (type.type == "FRIEND_LIST")
                {
                    friendsUI.LoadFriendList(data);
                }
                else if (type.type == "PENDING_REQUESTS")
                {

                }
            }
        }
        [Serializable]
        private struct GetReceiveType
        {
            public string type;
        }

        public void Send(WebSocket socket, string jsonMessage)
        {
            try
            {
                if (socket != null && socket.IsAlive)
                {
                    // WebSocket을 통해 서버로 전송
                    socket.Send(jsonMessage);
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
                if (friendWebSocket == null)
                    return;

                if (friendWebSocket.IsAlive)
                    friendWebSocket.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
}