using GH;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Networking;
namespace SW
{
    public class VisitOtherSchool : MonoBehaviour
    {
        public GameObject content;
        public GameObject schoolPanelPrefab;
        public SchoolPanel[] schoolPanels;
        public SchoolPanel selected;
        public void SetPanel()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        public void OnVisitButtonClick()
        {
            gameObject.SetActive(false);
            DataManager.instance.mapType = DataManager.MapType.School;
            DataManager.instance.mapId = selected.id;
            PhotonNetMgr.instance.roomName = selected.schoolName.text;
            PhotonNetwork.LeaveRoom();
            PhotonNetMgr.instance.sceneNum = 1;
        }
        public void Search(string value)
        {
            // 삭제
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            selected = null;
            // 요청
            HttpManager httpManager = HttpManager.GetInstance();
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/school/" + value;
            info.onComplete = (DownloadHandler res) =>
            {
                SchoolListRes data = JsonUtility.FromJson<SchoolListRes>("{\"data\" : " + res.text + "}");
                schoolPanels = new SchoolPanel[data.data.Length];
                for (int i = 0; i < data.data.Length; i++)
                {
                    GameObject newPanel = Instantiate(schoolPanelPrefab, content.transform);
                    SchoolPanel comp = newPanel.GetComponent<SchoolPanel>();
                    schoolPanels[i] = comp;
                    comp.id = data.data[i].id;
                    comp.schoolName.text = data.data[i].schoolName;
                    comp.playerNum.text = data.data[i].onlineUserCount.ToString();
                    comp.button.onClick.AddListener(() =>
                    {
                        foreach (var each in schoolPanels)
                        {
                            each.outLine.enabled = false;
                        }
                        if (selected == comp)
                        {
                            selected = null;
                        }
                        else
                        {
                            comp.outLine.enabled = true;
                            selected = comp;
                        }
                    });
                }
            };
            StartCoroutine(httpManager.Get(info));
        }
        [Serializable]
        private class SchoolListRes
        {
            public School[] data;
        }
    }
}