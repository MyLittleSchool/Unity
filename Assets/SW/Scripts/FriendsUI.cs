using GH;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static SW.FriendsUI;
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

        // Start is called before the first frame update
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
            RefreshTab3(recommFriends);
            ChangeTab(0);
        }
        private void ClosePanel()
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
                for (int i = 0; i < contentsTabs[0].childCount; i++)
                {
                    if (contentsTabs[0].GetChild(i).Find("CheckButton").GetChild(1).gameObject.activeSelf == true)
                    {
                        FriendPanel comp = contentsTabs[0].GetChild(i).GetComponent<FriendPanel>();
                        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
                        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/remove?friendshipId=" + comp.friendshipId;
                        StartCoroutine(HttpManager.GetInstance().Delete(info));
                        Destroy(comp.gameObject);
                    }
                }
                ModifyOff();
                ChangeTab(tab);
            }
        }
        private int checkedNum;
        private void ModifyOff()
        {
            for (int i = 0; i < contentsTabs[0].childCount; i++)
            {
                contentsTabs[0].GetChild(i).Find("InviteButton").gameObject.SetActive(true);
                contentsTabs[0].GetChild(i).Find("ChatButton").gameObject.SetActive(true);
                contentsTabs[0].GetChild(i).Find("CheckButton").GetChild(1).gameObject.SetActive(false);
                modifyBtnState = ModifyBtnState.Modify;
                modifyBtnText.text = "관리하기";
            }
        }
        private int tab;
        private void ChangeTab(int num)
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
        private int selectedUserId;
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
            ChangeTab(tab);
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
                        // 교실 놀러가기 버튼
                        comp.PassButton.onClick.AddListener(() =>
                        {
                            DataManager.instance.mapType = DataManager.MapType.MyClassroom;
                            DataManager.instance.mapId = comp.id;
                            PhotonNetMgr.instance.roomName = friend.name;
                            PhotonNetwork.LeaveRoom();
                            PhotonNetMgr.instance.sceneNum = 2;
                        });
                        // 쪽지 버튼
                        comp.RequestButton.onClick.AddListener(() =>
                        {
                            selectedUserId = comp.id;
                            noteCreatePanel.gameObject.SetActive(true);
                            inputField.text = "";
                        });
                        // 체크 버튼
                        Transform btn = newPanel.transform.Find("CheckButton");
                        btn.GetComponent<Button>().onClick.AddListener(() =>
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
                    }
                }
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));

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
                            ChangeTab(tab);
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
            };
            StartCoroutine(HttpManager.GetInstance().Get(getinfo));

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
                            ChangeTab(tab);
                        });
                    }
                }
            };
            StartCoroutine(HttpManager.GetInstance().Get(waitInfo));
        }
        public void RefreshTab3(List<RecommFriend> _recommFriends)
        {
            // 삭제
            Destroy(contentsTabs[3].gameObject);
            // 생성
            contentsTabs[3] = Instantiate(tabPrefab, contents).transform;
            // 서버 요청
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/ai-recommend/name-list?userId=1";
            info.onComplete = (DownloadHandler res) =>
            {
                List<string> nameList = JsonConvert.DeserializeObject<List<string>>(res.text);
                foreach (var name in nameList)
                {
                    GameObject newPanel = Instantiate(recommFriendPrefab, contentsTabs[3]);
                    newPanel.GetComponent<FriendPanel>().NickNameText.text = name;
                }
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
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