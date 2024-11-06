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

        public RectTransform contentPanel;
        public GameObject playerPanelPrefab;

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
        }
        public void JoinReq()
        {
            GameObject newPanel = Instantiate(playerPanelPrefab, contentPanel);
            newPanel.transform.Find("NicknameText").GetComponent<TMP_Text>().text = PhotonNetwork.LocalPlayer.NickName;
        }
        public void GetJoin(int actorNumber, ref GameObject ui)
        {
            ui = Instantiate(playerPanelPrefab, contentPanel);
            ui.transform.Find("NicknameText").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName;
        }
        public void JoinRes(int actorNumber)
        {
            GameObject newPanel = Instantiate(playerPanelPrefab, contentPanel);
            newPanel.transform.Find("NicknameText").GetComponent<TMP_Text>().text = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName;
        }
    }
}