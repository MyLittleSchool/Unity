using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class RequestFriendPanel : MonoBehaviour
    {
        public int receiverId;
        [Header("Panel1")]
        public GameObject panel1;
        public Image[] selectButtonImages;
        public TMP_Text[] msgPresets;
        [Header("Panel2")]
        public GameObject panel2;
        public TMP_InputField msgInputField;

        private int selectedIdx = -1;
        public void OffPanel()
        {
            selectedIdx = -1;
            for (int i = 0; i < 3; i++)
            {
                selectButtonImages[i].color = Color.white;
            }
            panel2.SetActive(false);
            msgInputField.text = "";
            gameObject.SetActive(false);
        }
        public void SelectMessage(int idx)
        {
            selectedIdx = idx;
            for (int i = 0; i < 3; i++)
            {
                if (i == idx)
                    selectButtonImages[idx].color = new Color(0.9490197f, 0.5333334f, 0.2941177f);
                else
                    selectButtonImages[i].color = Color.white;
            }
        }
        public void OnInputPanel()
        {
            selectedIdx = 3;
            panel2.SetActive(true);
        }
        public void OnClickConfirmButton()
        {
            if (selectedIdx == -1)
                return;
            string msg;
            if (selectedIdx == 3)
            {
                msg = msgInputField.text;
            }
            else
            {
                msg = msgPresets[selectedIdx].text;
            }
            // 통신
            ToastMessage.OnMessage("친구 추가를 요청하였습니다");
            WebSocketManager.GetInstance().Send(WebSocketManager.GetInstance().friendWebSocket, "{\"type\": \"FRIEND_REQUEST\", \"requesterId\": " + AuthManager.GetInstance().userAuthData.userInfo.id + ", \"receiverId\": " + receiverId + ", \"message\": \"" + msg + "\"}");
            QuestManager.instance.QuestPatch(2);
            if (WebSocketManager.GetInstance().friendsUI != null && WebSocketManager.GetInstance().friendsUI.gameObject.activeSelf) WebSocketManager.GetInstance().friendsUI.RefreshFriends();
            OffPanel();
        }
    }
}