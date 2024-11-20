using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using SW;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;

namespace GH
{

    public class PhotonChatMgr : MonoBehaviourPun, IChatClientListener
    {
        public enum Loginstep
        {
            All,
            School,
            Private
        }

        public Loginstep currentLogin;

        //Input Chat InputField
        public TMP_InputField inputChat;
        // ChatItem Prefab
        public GameObject chatItemPrefab;
        public Color color;
        public string playerName;

        // ChatItem의 부모 Transfrom
        public RectTransform contentRectTransform;

        //전체적인 PhotonChat 기능을 가지고 있는 변수
        ChatClient chatClient;

        // 일반채팅채널
        public string currChannel = "All";


        //챗 로그 뷰
        public GameObject chatLogView;
        bool chatLogOn = false;

        //말풍선
        public GameObject malpungPanel;
        public TMP_Text malpungText;
        PlayerMalpung playerMalpung;

        public RectTransform chatMainPanelRecttransform;
        public RectTransform chatPanelRecttransform;

        public List<GameObject> chatList = new List<GameObject>();

        //채팅 채널 전환 버튼
        public Button chatChannel;
        private TMP_Text chatChannelText;

        public static PhotonChatMgr instance;
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
        void Start()
        {
            playerName = DataManager.instance.playerName;
            //
            //currChannel = DataManager.instance.playerSchool;
            // 엔터 쳤을 때 호출되는 함수 등록
            inputChat.onSubmit.AddListener(OnSubmit);

            // photon chat 서버에 접속
            PhotonChatConnect();

            chatChannel.onClick.AddListener(ChatChannelChange);
            chatChannelText = chatChannel.gameObject.GetComponentInChildren<TMP_Text>();
        }

        void Update()
        {
            // 채팅 서버에서 오는 응답을 수신하기 위해서 계속 호출 해줘야 한다.
            if (chatClient != null)
            {
                chatClient.Service();
            }
            //print(chatClient.PublicChannels[currChannel].Subscribers.Count);

            //플레이어의 말풍선을 찾는다.
            if (malpungPanel == null)
            {
                if (DataManager.instance && DataManager.instance.player)
                {
                    playerMalpung = DataManager.instance.player.GetComponent<PlayerMalpung>();
                    malpungPanel = playerMalpung.malpungPanel;
                    malpungText = playerMalpung.malpungText;
                }
            }

           
        }
        public void ChatChannelChange()
        {
            currentLogin = currentLogin == Loginstep.All ? Loginstep.School : Loginstep.All;
            switch (currentLogin)
            {
                case Loginstep.All:
                    JoinChatRoom(DataManager.instance.playerCurrChannel);
                    chatChannelText.text = "전체";
                    print("전체");

                    break;
                case Loginstep.School:
                    chatChannelText.text = "학교";
                    print("학교");
                    JoinChatRoom(AuthManager.GetInstance().userAuthData.userInfo.school.schoolName);
                    break;
                case Loginstep.Private:
                    break;
            }
        }

        void PhotonChatConnect()
        {
            print("커넥트 진입");
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
            chatClient.AuthValues = new Photon.Chat.AuthenticationValues(playerName);
            //연결시도
            chatClient.ConnectUsingSettings(chatAppSettings);
            print("연결 시도");

        }

        public void JoinChatRoom(string newRoom)
        {
            //채팅 로그 초기화
            for (int i = 0; i < chatList.Count; i++)
            {
                Destroy(chatList[i]);
            }

            chatList.Clear();
            if (!string.IsNullOrEmpty(currChannel))
            {
                chatClient.Unsubscribe(new string[] { currChannel });
            }
            chatClient.Disconnect();

            currChannel = newRoom;
            PhotonChatConnect();
            print(newRoom + "룸 만들었다!");
            //chatClient.Subscribe(new string[] { currChannel });
        }
        public enum ChatMode
        {
            Default, AIChatBot
        }
        public ChatMode ChatModeState { get; set; }
        public AIChatBotNPC NPC { get; set; }
        private void OnSubmit(string s)
        {
            // 채팅창에 아무것도 없으면 함수를 끝낸다.
            if (inputChat.text.Length < 1)
                return;
            // 닉네임의 색을 변경 Color로
            //string nickName;
            //if (playerName == DataManager.instance.playerName)
            //{
            //    nickName = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + playerName + "</color>";
            //}
            //else
            //{
            //    nickName = playerName;
            //}

            if (ChatModeState == ChatMode.Default)
            {
                ChatLogSeverPost(s);

                //귓속말인지 판단
                // /w 아이디 메시지면 귓속말
                string[] text = s.Split(" ", 2);
                string chat = playerName + "  " + s;

                //귓속말 
                if (s[0] == '@')
                {
                    chat = text[0] + "  " + text[1];
                    //귓속말 보내기
                    chatClient.SendPrivateMessage(text[0].Remove(0, 1), chat);
                    print(text[0].Remove(0, 1) + "에게 귓속말");
                    inputChat.text = "";
                }
                else
                {
                    // 일반채팅을 보내자
                    chatClient.PublishMessage(currChannel, chat);
                    //말풍선에 텍스트를 넣는다.
                    playerMalpung.RPC_MalPungText(inputChat.text);
                    inputChat.text = "";
                }
            }
            else if (ChatModeState == ChatMode.AIChatBot)
            {
                NPC.ReqChat(s);
                playerMalpung.MalPungText(s);
                inputChat.text = "";
            }
        }

        private void CreateChatItem(string chat, Color chatColor)
        {

            //s의 내용을 ChatItem을 만들자
            GameObject go = Instantiate(chatItemPrefab, contentRectTransform);
            ChatItem chatItem = go.GetComponent<ChatItem>();
            chatList.Add(go);


            // 가져온 컴포넌트의 SetText함수를 실행
            chatItem.SetText(chat, chatColor);
            //content Size Fitter 트랜스폼 새로고침
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }
        public void OnChatStateChange(ChatState state)
        {
            Debug.Log($"Chat State Changed: {state}");
        }

        public void OnErrorInfo(ErrorInfo errorInfo)
        {
            Debug.LogError($"Error: {errorInfo.Info}");
        }
        public void OnDisconnected()
        {
            PhotonChatConnect();
            print("실패!!!!!!");
        }

        //채팅 서버에 접속 성공하면 호출되는 함수
        public void OnConnected()
        {
            print("채팅 서버 접속 성공!");
            // 특정 채널에 들어가자(구독)

            chatClient.Subscribe(currChannel, 0, -1, new ChannelCreationOptions() { PublishSubscribers = true });


        }

      

        // 특정 채널에 메시지가 들어올 때 호출되는 함수
        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                // 채팅 아이템 만들어서 스크롤 뷰에 올리기
                CreateChatItem(messages[i].ToString(), color);
            }
        }

        //귓속말
        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            //채팅 아이템 만들어서 스크롤 뷰에 붙이자
            CreateChatItem(message.ToString(), color);
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
            chatHeight = chatLogOn ? 1000 : 0;
            //chatLogView.GetComponentAtIndex<Image>(0).raycastTarget = chatLogOn ? true : false;
            chatRectTransform.GetChild(0).GetComponent<Image>().raycastTarget = chatLogOn ? true : false;
            chatRectTransform.sizeDelta = new Vector2(chatRectTransform.sizeDelta.x, chatHeight);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatMainPanelRecttransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatPanelRecttransform);



        }

        public void ChatLogSeverPost(string s)
        {
            ChatLogInfo chatLogInfo = new ChatLogInfo();
            chatLogInfo.message = s;
            //chatLogInfo.timestamp
            chatLogInfo.channel = currChannel;
            chatLogInfo.chatType = "PRIVATE";
            chatLogInfo.senderId = AuthManager.GetInstance().userAuthData.userInfo.id;

            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/chat-log";
            info.body = JsonUtility.ToJson(chatLogInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }


        //인풋창에 플레이어 이름 추가하기
        public void OneToOneChat(string playerName)
        {

            inputChat.text = "@" + playerName + " ";
        }

        // 채팅 입력 On
        public void OnChatInput()
        {
            inputChat.Select();
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
        }

        public void PrivateRoomIn(string s)
        {
            JoinChatRoom(s);
            chatChannelText.text = s;
            print(s);
        }
    }

    [Serializable]
    public struct ChatLogInfo
    {
        public int senderId;
        public int receiverId;
        public string message;
        public string timestamp;
        public string channel;
        public string chatType;

    }

}