using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class FriendsUI : MonoBehaviour
    {
        public Button closeButton;
        public RectTransform contents;
        public GameObject friendPrefab;

        public List<Friend> friends;
        [Serializable]
        public class Friend
        {
            public int userId;
            public string nickname;
            public bool online;
        }
        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.AddListener(() => ClosePanel());

            RefreshList(friends);
        }
        private void ClosePanel()
        {
            gameObject.SetActive(false);
        }
        public void RefreshList(List<Friend> _friends)
        {
            // 삭제
            for (int i = contents.childCount - 1; i >= 0; i--)
            {
                Destroy(contents.GetChild(i).gameObject);
            }
            // 생성
            for (int i = 0; i < friends.Count; i++)
            {
                GameObject newPanel = Instantiate(friendPrefab, contents);
                newPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = friends[i].nickname;
            }
        }
    }
}