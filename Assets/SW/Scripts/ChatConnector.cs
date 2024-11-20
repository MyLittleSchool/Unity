using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Text;
using System;
using WebSocketSharp;

namespace SW
{
    public class ChatConnector : MonoBehaviour
    {
        public string IP = "192.168.0.23";
        public string PORT = "8080";

        //서버 서비스 이름
        public string SERVICE_NAME = "/ws/chat";

        public WebSocket webSocket = null;
        private void Awake()
        {

            try
            {
                webSocket = new WebSocket("ws://" + IP + ":" + PORT + SERVICE_NAME);
                webSocket.OnOpen += (sender, e) =>
                {
                    Debug.Log("서버와 연결되었습니다.");
                };
                webSocket.OnMessage += Recv;
                //webSocket.OnClose += CloseConnect;
                // 연결이 닫혔을 때 호출되는 콜백
                webSocket.OnClose += (sender, e) =>
                {
                    Debug.Log("서버와의 연결이 종료되었습니다.");
                };

                // 오류 발생 시 호출되는 콜백
                webSocket.OnError += (sender, e) =>
                {
                    Debug.LogError("WebSocket 오류: " + e.Message);
                };
                //서버 연결 시도
                Connect();

            }
            catch (Exception ex)
            {
                Debug.LogError("WebSocket 연결 중 오류 발생: " + ex.Message);
            }
            //SendMessage(JsonUtility.ToJson(ChatInfo));
        }



        //서버 연결함수
        public void Connect()
        {
            try
            {
                if (webSocket == null || !webSocket.IsAlive)
                    webSocket.Connect();

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private void CloseConnect(object sender, CloseEventArgs e)
        {
            DisconncectServer();
        }
        //연결 해제 함수
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


        //서버로 부터 받은 데이터 처리
        public void Recv(object sender, MessageEventArgs e)
        {
            // 서버에서 받은 데이터를 JSON 형식으로 처리

            //string 데이터
            //Debug.Log("서버로부터 받은 메시지 : " + e.Data);

            //bytes 데이터
            //Debug.Log(e.RawData);

            HandleServerMessage(e.Data);
            //받았을 때 새로고침===================================================================================================\ 
        }

        private void OnApplicationQuit()
        {
            DisconncectServer();
        }
        void OnMessageReceived(string mesassge)
        {
            // 웹소켓 통신이 비동기적으로 처리되었다면, UI 업데이트는 메인 스레드에서 해야 함
            //UnityMainThred_GH.Instance().Enqueue(mesassge);
        }

        IEnumerator Co_Test(ChatMessage receivedMessage)
        {
            //for (int i = 0; i < ChatManager_GH.instance.chatList.Count; i++)
            //{
            //    ChatData_GH chatData = ChatManager_GH.instance.chatList[i].GetComponent<ChatData_GH>();
            //    //각 채팅방에 맞는 정보 넘기기
            //    if (receivedMessage.roomId == chatData.chatInfo.roomID)
            //    {
            //        //웹소켓에서 넘어온 룸 아이디에 맞는 룸 아이디 정보들을 비교하고 같은 룸 아이디의 채팅 방을 찾아서 그곳에 있는 룸 데이터의 인스탠티에이트 함수를 호출하여 생성한다?
            //        chatData.ReceiveMessage(receivedMessage);

            //    }

            //}

            yield return null;
        }

        // 서버에서 받은 메시지를 처리하는 메서드
        private void HandleServerMessage(string jsonData)
        {
            try
            {
                // 서버에서 받은 메시지를 ChatMessage 객체로 변환
                ChatMessage receivedMessage = JsonUtility.FromJson<ChatMessage>(jsonData);

                OnMessageReceived(jsonData);

                //OnMessageReceived(receivedMessage);

                //for (int i = 0; i < ChatManager_GH.instance.chatList.Count; i++)
                //{
                //    ChatData_GH chatData = ChatManager_GH.instance.chatList[i].GetComponent<ChatData_GH>();
                //    //각 채팅방에 맞는 정보 넘기기
                //    if (receivedMessage.roomId == chatData.chatInfo.roomID)
                //    {
                //        //웹소켓에서 넘어온 룸 아이디에 맞는 룸 아이디 정보들을 비교하고 같은 룸 아이디의 채팅 방을 찾아서 그곳에 있는 룸 데이터의 인스탠티에이트 함수를 호출하여 생성한다?
                //        chatData.ReceiveMessage(receivedMessage);

                //    }

                //}

                // 채팅창에 메시지 누적
                //for (int i = 0; i < ChatManager_GH.instance.chatList.Count; i++)
                //{
                //    ChatData_GH chatData = ChatManager_GH.instance.chatList[i].GetComponent<ChatData_GH>();
                //    //각 채팅방에 맞는 정보 넘기기
                //    if (receivedMessage.roomId == chatData.chatInfo.roomID)
                //    {
                //        //웹소켓에서 넘어온 룸 아이디에 맞는 룸 아이디 정보들을 비교하고 같은 룸 아이디의 채팅 방을 찾아서 그곳에 있는 룸 데이터의 인스탠티에이트 함수를 호출하여 생성한다?
                //        chatData.Test(receivedMessage);

                //    }

                //}
                // 받은 메시지 출력
                Debug.Log(receivedMessage.userId + " : " + receivedMessage.message);

            }
            catch (Exception ex)
            {
                Debug.LogError("JSON 데이터 처리 중 오류 발생: " + ex.Message);
                //여기서 오류 발생
                //get_isActiveAndEnabled can only be called from the main thread.
            }
        }



        // 서버로 메시지를 전송하는 메서드
        public void SendMessageToServer(string messageType, string roomId, string sender, string message)
        {
            try
            {
                if (webSocket != null && webSocket.IsAlive)
                {
                    // ChatMessage 객체 생성
                    ChatMessage chatMessage = new ChatMessage(messageType, roomId, sender, message);


                    // 객체를 JSON 문자열로 변환
                    string jsonMessage = JsonUtility.ToJson(chatMessage);

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
        // MonoBehaviour의 OnDestroy에서 WebSocket 연결 종료
        void OnDestroy()
        {
            try
            {
                if (webSocket != null)
                {
                    webSocket.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("WebSocket 종료 중 오류 발생: " + ex.Message);
            }
        }

    }

    [System.Serializable]
    public class ChatMessage
    {
        public string messageType;
        public string roomId;
        public string userId;
        public string message;

        public ChatMessage(string messageType, string roomId, string sender, string message)
        {
            this.messageType = messageType;
            this.roomId = roomId;
            this.userId = sender;
            this.message = message;
        }
    }
}