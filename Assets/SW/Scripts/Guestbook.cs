using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class Guestbook : MonoBehaviour
    {
        public Button closeButton;
        public List<RectTransform> contentsRaws = new List<RectTransform>();
        private void Start()
        {
            closeButton.onClick.AddListener(() => { OffPanel(); });

        }
        private void OffPanel()
        {
            gameObject.SetActive(false);
        }
        public void AddContent(int id, string content, string nickname, DateTime time, Color color)
        {

        }
    }
}