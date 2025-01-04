using GH;
using MJ;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static SW.FriendsUI;
using static Unity.Burst.Intrinsics.X86.Avx;
namespace SW
{
    public class FriendsUI : MonoBehaviour
    {
        public Button closeButton;
        public Button modifyButton;
        public TMP_Text modifyBtnText;
        public RectTransform contents;
        public GameObject friendPrefab;
        public GameObject requestedPrefab;
        public GameObject requestingPrefab;
        public GameObject recommFriendPrefab;
        public GameObject tabPrefab;
        public Button[] tabButtons;
        public Transform[] contentsTabs;
        public RectTransform noteCreatePanel;
        public TMP_Text numText;

        public List<Friend> friends;
        public List<RecommFriend> recommFriends;
        //public List<>
        [Serializable]
        public class Friend
        {
            public int userId;
            public string nickname;
            public bool online;
        }
        [Serializable]
        public class RecommFriend : Friend
        {
            public int grade;
            public string location;
            public string interest;
            public string message;
        }

        private void Awake()
        {
            WebSocketManager.GetInstance().friendsUI = this;
        }
        void Start()
        {
            closeButton.onClick.AddListener(() => ClosePanel());
            modifyButton.onClick.AddListener(() => ModifyButton());
            for (int i = 0; i < tabButtons.Length; i++)
            {
                int idx = i;
                tabButtons[i].onClick.AddListener(() => ChangeTab(idx));
            }
            foreach (Button button in colorButtons)
            {
                button.onClick.AddListener(() => { SetColor(button); });
            }

            //RefreshTab0(friends);
            //RefreshTab3(recommFriends);
            ChangeTab(0);
        }
        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
        public void CloseNoteCreatePanel()
        {
            noteCreatePanel.gameObject.SetActive(false);
        }
        enum ModifyBtnState
        {
            Modify, Cancel, Confirm
        }
        ModifyBtnState modifyBtnState;
        private void ModifyButton()
        {
            if (modifyBtnState == ModifyBtnState.Modify)
            {
                checkedNum = 0;
                modifyBtnState = ModifyBtnState.Cancel;
                modifyBtnText.text = "취소";
                for (int i = 0; i < contentsTabs[0].childCount; i++)
                {
                    contentsTabs[0].GetChild(i).Find("EnterButton").gameObject.SetActive(false);
                    contentsTabs[0].GetChild(i).Find("ChatButton").gameObject.SetActive(false);
                }
            }
            else if (modifyBtnState == ModifyBtnState.Cancel)
            {
                ModifyOff();
            }
            else if (modifyBtnState == ModifyBtnState.Confirm)
            {
                WebSocketManager webSocketManager = WebSocketManager.GetInstance();
                for (int i = 0; i < contentsTabs[0].childCount; i++)
                {
                    if (contentsTabs[0].GetChild(i).Find("CheckButton").GetChild(1).gameObject.activeSelf == true)
                    {
                        FriendPanel comp = contentsTabs[0].GetChild(i).GetComponent<FriendPanel>();
                        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
                        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/remove?friendshipId=" + comp.friendshipId;
                        //StartCoroutine(HttpManager.GetInstance().Delete(info));
                        webSocketManager.Send(webSocketManager.friendWebSocket, "{\"type\": \"DELETE_FRIEND_REQUEST\", \"friendshipId\": " + comp.friendshipId + "}");
                        Destroy(comp.gameObject);
                    }
                }
                ChangeTab(tab);
                ModifyOff();
            }
        }
        private int checkedNum;
        private void ModifyOff()
        {
            try
            {
                for (int i = 0; i < contentsTabs[0].childCount; i++)
                {
                    contentsTabs[0].GetChild(i).Find("EnterButton").gameObject.SetActive(true);
                    contentsTabs[0].GetChild(i).Find("ChatButton").gameObject.SetActive(true);
                    contentsTabs[0].GetChild(i).Find("CheckButton").GetChild(1).gameObject.SetActive(false);
                    modifyBtnState = ModifyBtnState.Modify;
                    modifyBtnText.text = "관리하기";
                }
            }
            catch { }
        }
        private int tab;
        public void ChangeTab(int num)
        {
            tab = num;
            if (num == 0)
            {
                modifyButton.gameObject.SetActive(true);
                numText.text = "친구 " + contentsTabs[num].transform.childCount.ToString("D2") + "명";
            }
            else
            {
                modifyButton.gameObject.SetActive(false);
                if (num == 1)
                {
                    numText.text = "내가 받은 요청 " + contentsTabs[num].transform.childCount.ToString("D2") + "명";
                }
                else if (num == 2)
                {
                    numText.text = "내가 보낸 요청 " + contentsTabs[num].transform.childCount.ToString("D2") + "명";
                }
                else if (num == 3)
                {   
                    numText.text = "추천 인원 " + contentsTabs[num].transform.childCount.ToString("D2") + "명";
                }
            }
            for (int i = 0; i < contentsTabs.Length; i++)
            {
                if (i == num)
                {
                    contentsTabs[i].gameObject.SetActive(true);
                    tabButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    contentsTabs[i].gameObject.SetActive(false);
                    tabButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        public void SetFriendStatus(string json, bool isOnline)
        {
            StatusInfo status = JsonUtility.FromJson<StatusInfo>(json);
            FriendPanel comp = friendDic[status.userId];
            SetFriendPanel(comp, isOnline, status.mapType, status.mapId);
        }
        private struct StatusInfo
        {
            public int userId;
            public int mapId;
            public string mapType;
        }
        public void SetFriendPanel(FriendPanel comp, bool isOnline, string mapType, int mapId)
        {
            // 체크 버튼
            Transform btn = comp.transform.Find("CheckButton");
            Button button = btn.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (btn.transform.GetChild(1).gameObject.activeSelf)
                {
                    checkedNum--;
                    if (checkedNum <= 0)
                    {
                        modifyBtnText.text = "취소";
                        modifyBtnState = ModifyBtnState.Cancel;
                    }
                }
                else
                {
                    checkedNum++;
                    modifyBtnText.text = "친구 삭제하기";
                    modifyBtnState = ModifyBtnState.Confirm;
                }
                btn.transform.GetChild(1).gameObject.SetActive(!btn.transform.GetChild(1).gameObject.activeSelf);
            });
            comp.RequestButton.onClick.RemoveAllListeners();
            comp.PassButton.onClick.RemoveAllListeners();
            if (isOnline)
            {
                // 위치
                if (mapType == DataManager.MapType.School.ToString())
                {
                    HttpManager.HttpInfo info = new HttpManager.HttpInfo();
                    info.url = HttpManager.GetInstance().SERVER_ADRESS + "/school?schoolId=" + mapId;
                    info.onComplete = (DownloadHandler res) =>
                    {
                        School school = JsonUtility.FromJson<School>(res.text);
                        comp.StateText.text = "<color=#F2884B>" + school.schoolName;
                        comp.PassButton.onClick.AddListener(() =>
                        {
                            SceneMgr.instance.SchoolIn(school.schoolName, mapId);
                            gameObject.SetActive(false);
                        });
                    };
                    StartCoroutine(HttpManager.GetInstance().Get(info));
                }
                else if (mapType == DataManager.MapType.MyClassroom.ToString())
                {
                    HttpManager.HttpInfo info = new HttpManager.HttpInfo();
                    info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/" + mapId;
                    info.onComplete = (DownloadHandler res) =>
                    {
                        UserInfo userInfo = JsonUtility.FromJson<UserInfo>(res.text);
                        comp.StateText.text = "<color=#F2884B>" + userInfo.name + "님의 교실";
                        comp.PassButton.onClick.AddListener(() =>
                        {
                            SceneMgr.instance.ClassIn(userInfo.name, mapId);
                            gameObject.SetActive(false);
                        });
                    };
                    StartCoroutine(HttpManager.GetInstance().Get(info));
                }
                else if (mapType == DataManager.MapType.Square.ToString())
                {
                    comp.StateText.text = "<color=#F2884B>만남의 광장";
                    comp.PassButton.onClick.AddListener(() =>
                    {
                        SceneMgr.instance.SquareIn();
                        gameObject.SetActive(false);
                    });
                }
                else if (mapType == DataManager.MapType.Quiz.ToString())
                {
                    comp.StateText.text = "<color=#F2884B>퀴즈";
                    comp.PassButton.onClick.AddListener(() =>
                    {
                        SceneMgr.instance.QuizSquareIn();
                        gameObject.SetActive(false);
                    });
                }
                else if (mapType == DataManager.MapType.QuizSquare.ToString())
                {
                    comp.StateText.text = "<color=#F2884B>퀴즈 광장";
                    comp.PassButton.onClick.AddListener(() =>
                    {
                        SceneMgr.instance.QuizSquareIn();
                        gameObject.SetActive(false);
                    });
                }
                else if (mapType == DataManager.MapType.ContestClassroom.ToString())
                {
                    HttpManager.HttpInfo info = new HttpManager.HttpInfo();
                    info.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/" + mapId;
                    info.onComplete = (DownloadHandler res) =>
                    {
                        UserInfo userInfo = JsonUtility.FromJson<UserInfo>(res.text);
                        comp.StateText.text = "<color=#F2884B>" + userInfo.name + "님의 맵 콘테스트";
                        comp.PassButton.onClick.AddListener(() =>
                        {
                            ToastMessage.OnMessage("따라갈 수 없는 위치에 있습니다");
                        });
                    };
                    StartCoroutine(HttpManager.GetInstance().Get(info));
                }
                else
                {
                    comp.StateText.text = "<color=#F2884B>접속중";
                    comp.PassButton.onClick.AddListener(() =>
                    {
                        ToastMessage.OnMessage("따라갈 수 없는 위치에 있습니다");
                    });
                }
                // 귓속말 보내기
                comp.RequestButton.GetComponentInChildren<TMP_Text>().text = "귓속말 보내기";
                comp.RequestButton.onClick.AddListener(() =>
                {
                    PhotonChatMgr.instance.OneToOneChat(comp.NickNameText.text);
                    gameObject.SetActive(false);
                });
                // 따라가기 버튼
                comp.PassButton.GetComponentInChildren<TMP_Text>().text = "따라가기";
            }
            else
            {
                comp.StateText.text = "";
                // 쪽지 버튼
                comp.RequestButton.GetComponentInChildren<TMP_Text>().text = "쪽지 남기기";
                comp.RequestButton.onClick.AddListener(() =>
                {
                    selectedUserId = comp.id;
                    noteCreatePanel.gameObject.SetActive(true);
                    inputField.text = "";
                });
                // 교실 놀러가기 버튼
                comp.PassButton.GetComponentInChildren<TMP_Text>().text = "교실 놀러가기";
                comp.PassButton.onClick.AddListener(() =>
                {
                    DataManager.instance.mapId = comp.id;
                    DataManager.instance.MapTypeState = DataManager.MapType.MyClassroom;
                    PhotonNetMgr.instance.roomName = comp.NickNameText.text;
                    gameObject.SetActive(false);
                    PhotonNetwork.LeaveRoom();
                    PhotonNetMgr.instance.sceneNum = 2;
                });
            }
        }
        private int selectedUserId;
        private Dictionary<int, FriendPanel> friendDic;
        public void RefreshFriends()
        {
            modifyBtnState = ModifyBtnState.Modify;
            modifyBtnText.text = "관리하기";
            // 삭제
            Destroy(contentsTabs[0].gameObject);
            Destroy(contentsTabs[1].gameObject);
            Destroy(contentsTabs[2].gameObject);
            // 생성
            contentsTabs[0] = Instantiate(tabPrefab, contents).transform;
            contentsTabs[1] = Instantiate(tabPrefab, contents).transform;
            contentsTabs[2] = Instantiate(tabPrefab, contents).transform;
            friendDic = new Dictionary<int, FriendPanel>();
            // 서버 요청
            // 내 친구 목록
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/list?userId=" + AuthManager.GetInstance().userAuthData.userInfo.id;
            info.onComplete = (DownloadHandler res) =>
            {
                FriendshipList list = JsonUtility.FromJson<FriendshipList>(res.text);
                if (list.response != null)
                {
                    for (int i = 0; i < list.response.Length; i++)
                    {
                        GameObject newPanel = Instantiate(friendPrefab, contentsTabs[0]);
                        FriendPanel comp = newPanel.GetComponent<FriendPanel>();
                        UserInfo friend = list.response[i].requester.id == AuthManager.GetInstance().userAuthData.userInfo.id ? list.response[i].receiver : list.response[i].requester;
                        comp.friendshipId = list.response[i].id;
                        comp.id = friend.id;
                        comp.NickNameText.text = friend.name;
                        friendDic[comp.id] = comp;
                        //SetFriendPanel(comp, friend.isOnline, friend.mapType, friend.mapId);
                    }
                }
                if (tab == 0) ChangeTab(tab);
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[0].GetComponent<RectTransform>());
            };
            //StartCoroutine(HttpManager.GetInstance().Get(info));

            // 친구 요청 목록
            HttpManager.HttpInfo getinfo = new HttpManager.HttpInfo();
            getinfo.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/list-receiver-unaccepted/" + AuthManager.GetInstance().userAuthData.userInfo.id;
            getinfo.onComplete = (DownloadHandler res) =>
            {
                FriendshipList list = JsonUtility.FromJson<FriendshipList>(res.text);
                if (list.response != null)
                {
                    for (int i = 0; i < list.response.Length; i++)
                    {
                        GameObject newPanel = Instantiate(requestedPrefab, contentsTabs[1]);
                        FriendPanel comp = newPanel.GetComponent<FriendPanel>();
                        UserInfo requester = list.response[i].requester;
                        comp.friendshipId = list.response[i].id;
                        comp.id = requester.id;
                        comp.NickNameText.text = requester.name;
                        comp.GradeText.text = requester.grade + "학년";
                        comp.locationText.text = requester.school.schoolName;
                        comp.InterestText.text = "#" + String.Join(" #", requester.interest);
                        //if (requester.isOnline)
                        //{
                        //    comp.StateText.text = "<color=#F2884B>접속중";
                        //}
                        //else
                        //{
                            comp.StateText.text = "";
                        //}
                        // 거절
                        comp.PassButton.onClick.AddListener(() =>
                        {
                            HttpManager.HttpInfo info3 = new HttpManager.HttpInfo();
                            info3.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/reject?friendshipId=" + comp.friendshipId;
                            info3.onComplete = (DownloadHandler res) =>
                            {
                            };
                            StartCoroutine(HttpManager.GetInstance().Post(info3));
                            Destroy(newPanel);
                            StartCoroutine(ChangeTab1FrameAfter());
                        });
                        // 수락
                        comp.RequestButton.onClick.AddListener(() =>
                        {
                            HttpManager.HttpInfo info3 = new HttpManager.HttpInfo();
                            info3.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/accept?friendshipId=" + comp.friendshipId;
                            info3.onComplete = (DownloadHandler res) =>
                            {
                                RefreshFriends();
                            };
                            StartCoroutine(HttpManager.GetInstance().Post(info3));
                        });
                        // 친구 요청 사유 추가 필요
                    }
                }
                if (tab == 1) ChangeTab(tab);
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[1].GetComponent<RectTransform>());
            };
            //StartCoroutine(HttpManager.GetInstance().Get(getinfo));

            // 요청 대기 목록
            HttpManager.HttpInfo waitInfo = new HttpManager.HttpInfo();
            waitInfo.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/list-requester-unaccepted/" + AuthManager.GetInstance().userAuthData.userInfo.id;
            waitInfo.onComplete = (DownloadHandler res) =>
            {
                FriendshipList list = JsonUtility.FromJson<FriendshipList>(res.text);
                if (list.response != null)
                {
                    for (int i = 0; i < list.response.Length; i++)
                    {
                        GameObject newPanel = Instantiate(requestingPrefab, contentsTabs[2]);
                        FriendPanel comp = newPanel.GetComponent<FriendPanel>();
                        UserInfo receiver = list.response[i].receiver;
                        comp.friendshipId = list.response[i].id;
                        comp.id = receiver.id;
                        comp.NickNameText.text = receiver.name;
                        if (receiver.isOnline)
                        {
                            comp.StateText.text = "<color=#F2884B>접속중";
                        }
                        else
                        {
                            comp.StateText.text = "";
                        }
                        // 요청취소
                        comp.RequestButton.onClick.AddListener(() =>
                        {
                            HttpManager.HttpInfo info3 = new HttpManager.HttpInfo();
                            info3.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/cancel?friendshipId=" + comp.friendshipId;
                            info3.onComplete = (DownloadHandler res) =>
                            {
                            };
                            StartCoroutine(HttpManager.GetInstance().Post(info3));
                            Destroy(newPanel);
                            StartCoroutine(ChangeTab1FrameAfter());
                        });
                    }
                }
                if (tab == 2) ChangeTab(tab);
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[2].GetComponent<RectTransform>());
            };
            //StartCoroutine(HttpManager.GetInstance().Get(waitInfo));

            // 소켓
            WebSocketManager webSocketManager = WebSocketManager.GetInstance();
            // 친구 목록 조회
            webSocketManager.Send(webSocketManager.friendWebSocket, "{\"type\": \"FETCH_FRIEND_LIST\", \"userId\": " + AuthManager.GetInstance().userAuthData.userInfo.id + "}");
            // 받은 요청 목록 조회
            webSocketManager.Send(webSocketManager.friendWebSocket, "{\"type\": \"FETCH_PENDING_REQUESTS\", \"userId\": " + AuthManager.GetInstance().userAuthData.userInfo.id + "}");
            // 보낸 요청 목록 조회
            webSocketManager.Send(webSocketManager.friendWebSocket, "{\"type\": \"FETCH_PENDING_REQUESTS_BY_REQUESTER\", \"userId\": " + AuthManager.GetInstance().userAuthData.userInfo.id + "}");
            // AI 추천
            RefreshTab3(recommFriends);
        }
        [Serializable]
        public class FriendList
        {
            public string type;
            public Friendship[] friends;
        }
        public void LoadFriendList(string res)
        {
            FriendList list = JsonUtility.FromJson<FriendList>(res);
            if (list.friends != null)
            {
                for (int i = 0; i < list.friends.Length; i++)
                {
                    //친구 리스트------------------------------------------------------------------
                    GameObject newPanel = Instantiate(friendPrefab, contentsTabs[0]);
                    FriendPanel comp = newPanel.GetComponent<FriendPanel>();
                    UserInfo friend = list.friends[i].requester.id == AuthManager.GetInstance().userAuthData.userInfo.id ? list.friends[i].receiver : list.friends[i].requester;
                    comp.reportButton.onClick.AddListener(() =>
                    {
                        SceneUIManager.GetInstance().OnProfilePanel(friend);
                    });
                    comp.friendshipId = list.friends[i].id;
                    comp.id = friend.id;
                    comp.NickNameText.text = friend.name;
                    friendDic[comp.id] = comp;
                    SetFriendPanel(comp, friend.isOnline, friend.mapType, friend.mapId);
                    // 규현 확인 
                    comp.ProfileImage.AvatarGet(friend.id);


                }
            }
            if (tab == 0) ChangeTab(tab);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[0].GetComponent<RectTransform>());
        }
        [Serializable]
        public class FriendRequestList
        {
            public string type;
            public Friendship[] requests;
        }
        public void LoadFriendRequest(string res)
        {
            WebSocketManager webSocketManager = WebSocketManager.GetInstance();
            FriendRequestList list = JsonUtility.FromJson<FriendRequestList>(res);
            if (list.requests != null)
            {
                for (int i = 0; i < list.requests.Length; i++)
                {
                    //친구 리스트------------------------------------------------------------------

                    UserInfo requester = list.requests[i].requester;
                    // 내가 받은 요청
                    GameObject newPanel = Instantiate(requestedPrefab, contentsTabs[1]);
                    FriendPanel comp = newPanel.GetComponent<FriendPanel>();
                    comp.reportButton.onClick.AddListener(() =>
                    {
                        SceneUIManager.GetInstance().OnProfilePanel(requester);
                    });
                    comp.friendshipId = list.requests[i].id;
                    comp.id = requester.id;
                    comp.NickNameText.text = requester.name;
                    comp.GradeText.text = requester.grade + "학년";
                    comp.locationText.text = requester.school.location + " " + requester.school.schoolName;
                    comp.InterestText.text = "#" + String.Join(" #", requester.interest);
                    comp.ProfileImage.AvatarGet(requester.id);
                    comp.MessageText.text = list.requests[i].message;
                    if (requester.isOnline)
                    {
                        comp.StateText.text = "<color=#F2884B>접속중";
                    }
                    else
                    {
                        comp.StateText.text = "";
                    }
                    // 거절
                    comp.PassButton.onClick.AddListener(() =>
                    {
                        //HttpManager.HttpInfo info3 = new HttpManager.HttpInfo();
                        //info3.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/reject?friendshipId=" + comp.friendshipId;
                        //StartCoroutine(HttpManager.GetInstance().Post(info3));
                        webSocketManager.Send(webSocketManager.friendWebSocket, "{\"type\": \"FRIEND_REJECT\", \"friendshipId\": " + comp.friendshipId + "}");
                        Destroy(newPanel);
                        StartCoroutine(ChangeTab1FrameAfter());
                    });
                    // 수락
                    comp.RequestButton.onClick.AddListener(() =>
                    {
                        HttpManager.HttpInfo info3 = new HttpManager.HttpInfo();
                        info3.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/accept?friendshipId=" + comp.friendshipId;
                        info3.onComplete = (DownloadHandler res) =>
                        {
                            RefreshFriends();
                        };
                        //StartCoroutine(HttpManager.GetInstance().Post(info3));
                        webSocketManager.Send(webSocketManager.friendWebSocket, "{\"type\": \"FRIEND_ACCEPT\", \"friendshipId\": " + comp.friendshipId + "}");
                    });
                    // 친구 요청 사유 추가 필요
                }
            }
            if (tab == 1) ChangeTab(tab);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[1].GetComponent<RectTransform>());
        }
        public void LoadFriendRequesting(string res)
        {
            WebSocketManager webSocketManager = WebSocketManager.GetInstance();
            FriendRequestList list = JsonUtility.FromJson<FriendRequestList>(res);
            if (list.requests != null)
            {
                for (int i = 0; i < list.requests.Length; i++)
                {
                    //친구 리스트------------------------------------------------------------------
                    // 내가 보낸 요청
                    GameObject newPanel = Instantiate(requestingPrefab, contentsTabs[2]);
                    FriendPanel comp = newPanel.GetComponent<FriendPanel>();
                    UserInfo receiver = list.requests[i].receiver;
                    comp.reportButton.onClick.AddListener(() =>
                    {
                        SceneUIManager.GetInstance().OnProfilePanel(receiver);
                    });
                    comp.friendshipId = list.requests[i].id;
                    comp.id = receiver.id;
                    comp.NickNameText.text = receiver.name;
                    comp.GradeText.text = receiver.grade + "학년";
                    comp.locationText.text = receiver.school.location + " " + receiver.school.schoolName;
                    comp.InterestText.text = "#" + String.Join(" #", receiver.interest);
                    comp.ProfileImage.AvatarGet(receiver.id);
                    comp.MessageText.text = list.requests[i].message;
                    if (receiver.isOnline)
                    {
                        comp.StateText.text = "<color=#F2884B>접속중";
                    }
                    else
                    {
                        comp.StateText.text = "";
                    }
                    // 요청취소
                    comp.RequestButton.onClick.AddListener(() =>
                    {
                        HttpManager.HttpInfo info3 = new HttpManager.HttpInfo();
                        info3.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/cancel?friendshipId=" + comp.friendshipId;
                        //StartCoroutine(HttpManager.GetInstance().Post(info3));
                        webSocketManager.Send(webSocketManager.friendWebSocket, "{\"type\": \"DELETE_FRIEND_REQUEST\", \"friendshipId\": " + comp.friendshipId + "}");
                        Destroy(newPanel);
                        StartCoroutine(ChangeTab1FrameAfter());
                    });
                    if (tab == 2) ChangeTab(tab);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[2].GetComponent<RectTransform>());
                }
            }
            if (tab == 2) ChangeTab(tab);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[1].GetComponent<RectTransform>());
        }

        public void RefreshTab3(List<RecommFriend> _recommFriends)
        {
            print("test1");
            // 삭제
            Destroy(contentsTabs[3].gameObject);
            // 생성
            contentsTabs[3] = Instantiate(tabPrefab, contents).transform;
            // 서버 요청
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/ai-recommend/list/recommendation-info?userId=" + AuthManager.GetInstance().userAuthData.userInfo.id;
            info.onComplete = (DownloadHandler res) =>
            {
                AIRecommendList list = JsonUtility.FromJson<AIRecommendList>("{\"data\" : " + res.text + "}");
                print("test1 :" + res.text);
                foreach (var each in list.data)
                {
                    //AI추천 리스트------------------------------------------------------------------

                    GameObject newPanel = Instantiate(recommFriendPrefab, contentsTabs[3]);
                    FriendPanel friendPanel = newPanel.GetComponent<FriendPanel>();
                    friendPanel.id = each.recommendedUserId;
                    friendPanel.NickNameText.text = each.username;
                    friendPanel.SimilarityText.text = "유사도 " + each.similarityValue.ToString("F2");
                    friendPanel.StateText.text = each.onlineStatus ? "접속중" : "";
                    friendPanel.RecommandText.text = each.recommendationLevel;
                    friendPanel.GradeText.text = each.grade + " 학년";
                    friendPanel.locationText.text = each.schoolLocation;
                    friendPanel.InterestText.text = "#" + String.Join(" #", each.interests);
                    friendPanel.MessageText.text = each.similarityMessage;
                    friendPanel.ProfileImage.AvatarGet(each.recommendedUserId);
                    friendPanel.PassButton.onClick.AddListener(() =>
                    {
                        Destroy(newPanel);
                        StartCoroutine(ChangeTab1FrameAfter());
                    });
                    friendPanel.RequestButton.onClick.AddListener(() =>
                    {
                        WebSocketManager.GetInstance().OnRequestFriendPanel(friendPanel.id);
                    });
                    friendPanel.reportButton.onClick.AddListener(() =>
                    {
                        int userId = GetComponent<FriendPanel>().id;
                        HttpManager.HttpInfo info2 = new HttpManager.HttpInfo();
                        info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/user/" + userId;
                        info2.onComplete = (DownloadHandler res) =>
                        {
                            UserInfo userInfo = JsonUtility.FromJson<UserInfo>(res.text);
                            SceneUIManager.GetInstance().OnProfilePanel(userInfo);
                        };
                        StartCoroutine(HttpManager.GetInstance().Get(info));
                    });
                }
                if (tab == 3) ChangeTab(tab);
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentsTabs[3].GetComponent<RectTransform>());
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
        IEnumerator ChangeTab1FrameAfter()
        {
            yield return null;
            ChangeTab(tab);
        }
        [Serializable]
        public class AIRecommendList
        {
            public List<AIRecommend> data;
        }
        [Serializable]
        public class AIRecommend
        {
            public int recommendedUserId;
            public string username;
            public float similarityValue;
            public bool onlineStatus;
            public string recommendationLevel;
            public int grade;
            public string schoolLocation;
            public List<string> interests;
            public string similarityMessage;
            public string avatarImageUrl;
        }

        [Header("쪽지 보내기")]
        public Button[] colorButtons;
        public Button saveButton;
        public TMP_InputField inputField;
        private Color selectedColor = Color.white;
        private int selectedColorIdx = 0;
        public Image createPanelImage;
        private void SetColor(Button _button)
        {
            for (int i = 0; i < colorButtons.Length; i++)
            {
                if (colorButtons[i] == _button)
                {
                    colorButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                    selectedColorIdx = i;
                }
                else colorButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            Color buttonColor = _button.GetComponent<Image>().color;
            if (buttonColor == Color.white) createPanelImage.color = new Color(0.9529412f, 0.9607844f, 0.9764706f);
            else createPanelImage.color = buttonColor;
            selectedColor = buttonColor;
        }
        public void OnValueChanged(string value)
        {
            if (value.Length == 0) saveButton.interactable = false;
            else saveButton.interactable = true;
        }
        public void OnSendNoteButtonClick()
        {
            PostInfo postInfo = new PostInfo();
            postInfo.mapId = selectedUserId;
            postInfo.mapType = DataManager.MapType.Note;
            postInfo.content = inputField.text;
            postInfo.backgroundColor = selectedColorIdx;
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/guest-book";
            info.body = JsonConvert.SerializeObject(postInfo, new JsonSerializerSettings { Converters = { new StringEnumConverter() } });
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                ToastMessage.OnMessage("작성을 완료하였습니다");
                QuestManager.instance.QuestPatch(3);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
            CloseNoteCreatePanel();
        }
        [Serializable]
        public class FriendshipList
        {
            public bool success;
            public Friendship[] response;
            public Error error;
        }
        [Serializable]
        public class Friendship
        {
            public int id;
            public UserInfo requester;
            public UserInfo receiver;
            public string message;
            public bool accepted;
        }
        [Serializable]
        public class Error
        {
            public string message;
            public int status;
        }
    }
}