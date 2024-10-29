using System;
using System.Collections;
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
            //RefreshTab3(recommFriends);
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

        public void RefreshTab0(List<Friend> _friends)
        {
            // 삭제
            Destroy(contentsTabs[0].gameObject);
            // 생성
            contentsTabs[0] = Instantiate(tabPrefab, contents).transform;
            // 서버 요청
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/임시";
            info.body = JsonUtility.ToJson(null);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                for (int i = 0; i < _friends.Count; i++)
                {
                    GameObject newPanel = Instantiate(friendPrefab, contentsTabs[0]);
                    newPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = _friends[i].nickname;
                }
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
        public void RefreshTab3(List<RecommFriend> _recommFriends)
        {
            // 삭제
            Destroy(contentsTabs[3].gameObject);
            // 생성
            contentsTabs[3] = Instantiate(tabPrefab, contents).transform;
            // 서버 요청
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/api/recommendations/run-batch";
            info.body = JsonUtility.ToJson(null);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print(res.text);
                //for (int i = 0; i < _recommFriends.Count; i++)
                //{
                //    GameObject newPanel = Instantiate(friendPrefab, contentsTabs[3]);
                //    newPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = _recommFriends[i].nickname;
                //}
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
    }
}