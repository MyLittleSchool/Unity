using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace SW
{
    public class VisitOtherSchool : MonoBehaviour
    {
        public GameObject content;
        public GameObject schoolPanelPrefab;
        public void Search(string value)
        {
            // 삭제
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            // 요청
            HttpManager httpManager = HttpManager.GetInstance();
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/school/" + value;
            info.onComplete = (DownloadHandler res) =>
            {
                SchoolListRes data = JsonUtility.FromJson<SchoolListRes>("\"data\" : " + res.text + "}");
                for (int i = 0; i < data.data.Length; i++)
                {
                    GameObject newPanel = Instantiate(schoolPanelPrefab, content.transform);
                    SchoolPanel comp = newPanel.GetComponent<SchoolPanel>();
                    //comp.id = data.data[i].schoolId
                }
            };
            httpManager.Get(info);
        }
        [Serializable]
        private class SchoolListRes
        {
            public School[] data;
        }
    }
}