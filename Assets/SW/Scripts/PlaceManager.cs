using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PlaceManager : MonoBehaviour
    {
        HttpManager httpManager;
        private void Start()
        {
            httpManager = HttpManager.GetInstance();
        }
        
        [Serializable]
        public class ObjectInfo
        {
            public int objId; // 오브젝트 아이디
            public int x, y; // 좌표
            public int rot; // 회전
        }
        [Serializable] // 배치 요청 파라미터
        public class SetPlaceInfo : ObjectInfo
        {
            private int userId;
            private int mapId;
            public SetPlaceInfo(ObjectInfo objectInfo)
            {
                objId = objectInfo.objId;
                x = objectInfo.x;
                y = objectInfo.y;
                rot = objectInfo.rot;
                // 구현 필요
                userId = HttpManager.GetInstance().UserId;
                mapId = HttpManager.GetInstance().MapId;
            }
        }

        public void CreatePlace(ObjectInfo objectInfo)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/ground-furniture";
            info.body = JsonUtility.ToJson(new SetPlaceInfo(objectInfo));
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print("생성요청완료, id : " + res.text);
            };
            StartCoroutine(httpManager.Post(info));
        }
        [Serializable] // 배치 불러오기 응답
        public class GetPlaceResInfo
        {
            public List<GetPlaceInfo> data;
        }
        [Serializable] // 불러온 오브젝트 구조
        public class GetPlaceInfo : ObjectInfo
        {
            public int id;
            public int userId;
            public int mapId;
        }
        public void ReadPlace()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/ground-furniture/map?mapId=" + HttpManager.GetInstance().MapId;
            info.onComplete = (DownloadHandler res) =>
            {
                GetPlaceResInfo dataInfo = JsonUtility.FromJson<GetPlaceResInfo>("{\"data\":" + res.text + "}");
                foreach (GetPlaceInfo info in dataInfo.data)
                {
                    print(info.id + "/" + info.userId + "/" + info.mapId + "/" + info.objId + "/" + info.x + "/" + info.y + "/" + info.rot);
                }
            };
            StartCoroutine(httpManager.Get(info));
        }

        public void DeletePlace(int furnitureId)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/ground-furniture?furnitureId=" + furnitureId;
            info.onComplete = (DownloadHandler res) =>
            {
                print("제거완료");
            };
            StartCoroutine(httpManager.Delete(info));
        }
    }
}
