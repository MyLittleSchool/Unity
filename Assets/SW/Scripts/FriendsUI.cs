using GH;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace SW
{
    public class FriendsUI : MonoBehaviour
    {
        public Button closeButton;
        public RectTransform contents;
        public GameObject friendPrefab;
        public GameObject recommFriendPrefab;
        public GameObject tabPrefab;
        public Button[] tabButtons;
        public Transform[] contentsTabs;

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
            for (int i = 0; i < tabButtons.Length; i++)
            {
                int idx = i;
                tabButtons[i].onClick.AddListener(() => ChangeTab(idx));
            }

            //RefreshTab0(friends);
            RefreshTab3(recommFriends);
            ChangeTab(0);
        }
        private void ClosePanel()
        {
            gameObject.SetActive(false);
        }
        private void ChangeTab(int num)
        {
            for (int i = 0; i < contentsTabs.Length; i++)
            {
                if (i == num)
                {
                    contentsTabs[i].gameObject.SetActive(true);
                    tabButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    contentsTabs[i].gameObject.SetActive(false);
                    tabButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

        public void RefreshFriends()
        {
            // 昏力
            Destroy(contentsTabs[0].gameObject);
            // 积己
            contentsTabs[0] = Instantiate(tabPrefab, contents).transform;
            // 辑滚 夸没
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            print(AuthManager.GetInstance().userAuthData.userInfo.id);
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/list-receiver-accepted?receiverId=" + AuthManager.GetInstance().userAuthData.userInfo.id;
            info.onComplete = (DownloadHandler res) =>
            {
                print(res.text);
                FriendshipList receiverList = JsonUtility.FromJson<FriendshipList>(res.text);
                print(receiverList);
                print(receiverList.success);
                print(receiverList.response);
                if (receiverList.response != null)
                {
                    for (int i = 0; i < receiverList.response.Length; i++)
                    {
                        GameObject newPanel = Instantiate(friendPrefab, contentsTabs[0]);
                        newPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = receiverList.response[i].requester.name;
                    }
                }
                HttpManager.HttpInfo info2 = new HttpManager.HttpInfo();
                info2.url = HttpManager.GetInstance().SERVER_ADRESS + "/friendship/list-requester-accepted?receiverId=" + AuthManager.GetInstance().userAuthData.userInfo.id;
                info2.onComplete = (DownloadHandler res2) =>
                {
                    print(res2.text);
                    FriendshipList requesterList = JsonUtility.FromJson<FriendshipList>(res2.text);
                    if (requesterList.response != null)
                    {
                        for (int i = 0; i < requesterList.response.Length; i++)
                        {
                            GameObject newPanel = Instantiate(friendPrefab, contentsTabs[0]);
                            newPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = receiverList.response[i].receiver.name;
                        }
                    }
                };
                StartCoroutine(HttpManager.GetInstance().Get(info2));
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
        public void RefreshTab3(List<RecommFriend> _recommFriends)
        {
            // 昏力
            Destroy(contentsTabs[3].gameObject);
            // 积己
            contentsTabs[3] = Instantiate(tabPrefab, contents).transform;
            // 辑滚 夸没
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/ai-recommend/name-list?userId=1";
            info.onComplete = (DownloadHandler res) =>
            {
                List<string> nameList = JsonConvert.DeserializeObject<List<string>>(res.text);
                foreach (var name in nameList)
                {
                    GameObject newPanel = Instantiate(recommFriendPrefab, contentsTabs[3]);
                    newPanel.GetComponent<RecommFriendPanel>().NickNameText.text = name;
                }
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }

        public class FriendshipList
        {
            public bool success;
            public Friendship[] response;
            public Error error;
        }
        public class Friendship
        {
            public int id;
            public UserInfo requester;
            public UserInfo receiver;
            public bool accepted;
        }
        public class Error
        {
            public string message;
            public int status;
        }
    }
}