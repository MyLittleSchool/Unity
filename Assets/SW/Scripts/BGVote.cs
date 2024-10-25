using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class BGVote : MonoBehaviour
    {
        public TMP_Text[] valueTexts;
        public Button[] voteButton;
        public Button closeButton;

        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.AddListener(() => OnCloseButtonClick());
            for (int i = 0; i < voteButton.Length; i++)
            {
                int id = i + 1;
                voteButton[i].onClick.AddListener(() =>
                {
                    OnVoteButtonClick(id);
                });
            }
        }
        private void OnCloseButtonClick()
        {
            gameObject.SetActive(false);
        }
        public void UpdateValues()
        {
            foreach (var each in valueTexts)
            {
                each.text = "123";
            }
        }
        private void OnVoteButtonClick(int id)
        {
            print("id : " + id);
        }
    }
}