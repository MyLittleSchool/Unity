using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class PhotonChatMgr : MonoBehaviour, IChatClientListener
    {
        //Input Chat InputField
        public TMP_InputField inputChat;
        // ChatItem Prefab
        public GameObject chatItemPrefab;

        public Color color;
        public string nick;

        // ChatItem의 부모 Transfrom
        public RectTransform contentRectTransform;

        //전체적인 PhotonChat 기능을 가지고 있는 변ㅅ
        ChatClient chatClient;

        // 일반채팅채널
        public string currChannel = "메타";

        //챗 로그 뷰
        public GameObject chatLogView;
        bool chatLogOn = false;

        //말풍선
        public GameObject malpungPanel;
        public TMP_Text malpungText;
        //말풍선 시간
        private float currtMalpungTime = 5.0f;
        private float maxMalpungTime = 5.0f;

        void Start()
        {
            // 엔터 쳤을 때 호출되는 함수 등록
            inputChat.onSubmit.AddListener(OnSbmit);

            // photon chat 서버에 접속
            PhotonChatConnect();

            //말풍선 끄기
            malpungPanel.SetActive(false);

            // 말풍 텍스트 초기화
            malpungText.text = "";
        }

        void Update()
        {
            // 채팅 서버에서 오는 응답을 수신하기 위해서 계속 호출 해줘야 한다.
            if (chatClient != null)
            {
                chatClient.Service();
            }
            //print(chatClient.PublicChannels[currChannel].Subscribers.Count);

            OnMalpung();
        }

        void PhotonChatConnect()
        {
            //포톤 설정 가져오기
            AppSettings photonSettings = PhotonNetwork.PhotonServerSettings.AppSettings;

            // 위 설정을 가지고 ChatAppSettings 셋팅
            ChatAppSettings chatAppSettings = new ChatAppSettings();
            chatAppSettings.AppIdChat = photonSettings.AppIdChat;
            chatAppSettings.AppVersion = photonSettings.AppVersion;
            chatAppSettings.FixedRegion = photonSettings.FixedRegion;
            chatAppSettings.NetworkLogging = photonSettings.NetworkLogging;
            chatAppSettings.Protocol = photonSettings.Protocol;
            chatAppSettings.EnableProtocolFallback = photonSettings.EnableProtocolFallback;
            chatAppSettings.Server = photonSettings.Server;
            chatAppSettings.Port = (ushort)photonSettings.Port;
            chatAppSettings.ProxyServer = photonSettings.ProxyServer;

            // ChatClient 만들자
            chatClient = new ChatClient(this);
            // 닉네임 설정
            chatClient.AuthValues = new Photon.Chat.AuthenticationValues(nick);
            //연결시도
            chatClient.ConnectUsingSettings(chatAppSettings);

        }
        private void OnSbmit(string s)
        {
            // 닉네임의 색을 변경 Color로
            string nickName = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + nick + "</color>";


            //귓속말인지 판단
            // /w 아이디 메시지면 귓속말
            string[] text = s.Split(" ", 3);
            string chat = nickName + " : " + s;

            //전체 채팅 구성을 만들자.
            if (text[0] == "/w")
            {

                chat = nickName + " : " + text[2];
                //귓속말 보내기
                chatClient.SendPrivateMessage(text[1], chat);
            }
            else
            {

                // 일반채팅을 보내자
                chatClient.PublishMessage(currChannel, chat);
            }


            malpungText.text = inputChat.text;
            currtMalpungTime = 0;
            inputChat.text = "";
        }

        private void CreateChatItem(string chat, Color chatColor)
        {

            //s의 내용을 ChatItem을 만들자
            GameObject go = Instantiate(chatItemPrefab, contentRectTransform);
            ChatItem chatItem = go.GetComponent<ChatItem>();



            // 가져온 컴포넌트의 SetText함수를 실행
            chatItem.SetText(chat, chatColor);
            //content Size Fitter 트랜스폼 새로고침
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }

        public void OnDisconnected()
        {
        }

        //채팅 서버에 접속 성공하면 호출되는 함수
        public void OnConnected()
        {
            print("채팅 서버 접속 성공!");
            // 특정 채널에 들어가자(구독)
            chatClient.Subscribe(currChannel, 0, -1, new ChannelCreationOptions() { PublishSubscribers = true });
        }

        public void OnChatStateChange(ChatState state)
        {
        }

        // 특정 채널에 메시지가 들어올 때 호출되는 함수
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                // 채팅 아이템 만들어서 스크롤 뷰에 올리기
                CreateChatItem(messages[i].ToString(), Color.black);
            }
        }

        //귓속말
        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            //채팅 아이템 만들어서 스크롤 뷰에 붙이자
            CreateChatItem(message.ToString(), new Color32(255, 0, 255, 255));
        }

        //채널을 참여할 때
        public void OnSubscribed(string[] channels, bool[] results)
        {
        }

        //채널에서 나올 때
        public void OnUnsubscribed(string[] channels)
        {
        }

        //친구 접속 상태가 바뀌면 호출
        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
        }

        //누군가 내가 있는 채널에 들어왔을 때 호출되는 함수
        public void OnUserSubscribed(string channel, string user)
        {
        }

        //누군가 내가 있는 채널에서 나갔을 때 호출되는 함수
        public void OnUserUnsubscribed(string channel, string user)
        {

        }

        //채팅로그 키기
        public void ChatLogActive()
        {
            RectTransform chatRectTransform = chatLogView.GetComponent<RectTransform>();
            float chatHeight = 0;
            // 챗로그창이 꺼져있으면 키고 껴져있으면 킨다.
            chatLogOn = chatLogOn ? false : true;

            //챗 로그의 높이로 챗 로그를 활성화 * 엑티브를 끄면 스크립트를 못가져와서 에러가 난다.
            chatHeight = chatLogOn ? 500 : 0;

            chatRectTransform.sizeDelta = new Vector2(chatRectTransform.sizeDelta.x, chatHeight);
        }

        //말풍선 생기기
        private void OnMalpung()
        {
            currtMalpungTime += Time.deltaTime;

            if (currtMalpungTime < maxMalpungTime)
            {
                malpungPanel.SetActive(true);
            }
            else
            {
                malpungPanel.SetActive(false);
            }
        }

    }

}