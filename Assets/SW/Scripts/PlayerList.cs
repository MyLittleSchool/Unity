using GH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace SW
{
    public class PlayerList : MonoBehaviourPun
    {
        public static PlayerList instance;

        public RectTransform contentPanel;
        public GameObject playerPanelPrefab;
        public TMP_Text numText;

        private int playerNum;
        private int PlayerNum
        {
            get { return playerNum; }
            set
            {
                playerNum = value;
                numText.text = playerNum.ToString("D2") + "Έν";
            }
        }
        public void SetPlayerListPanel()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        public void ResetContents()
        {
            for (int i = 0; i < contentPanel.transform.childCount; i++)
            {
                Destroy(contentPanel.transform.GetChild(i).gameObject);
            }
            PlayerNum = 1;
        }
        public PlayerPanel JoinReq()
        {
            GameObject newPanel = Instantiate(playerPanelPrefab, contentPanel);
            newPanel.transform.Find("NicknameText").GetComponent<TMP_Text>().text = PhotonNetwork.LocalPlayer.NickName;
            for (int i = 4; i < newPanel.transform.childCount; i++)
            {
                newPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
            return newPanel.GetComponent<PlayerPanel>();
        }
        public void GetJoin(int actorNumber, ref GameObject ui)
        {
            PlayerNum++;
            ui = Instantiate(playerPanelPrefab, contentPanel);
            ui.transform.Find("NicknameText").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName;
        }
        public void JoinRes(int actorNumber, ref GameObject ui)
        {
            PlayerNum++;
            ui = Instantiate(playerPanelPrefab, contentPanel);
            ui.transform.Find("NicknameText").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName;
        }
        public void Leave()
        {
            PlayerNum--;
        }
    }
}