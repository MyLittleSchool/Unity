using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PlaceManager : MonoBehaviour
    {
        HttpManager httpManager = HttpManager.GetInstance();

        [Serializable]
        public class ObjectInfo
        {
            public int objId; // 오브젝트 아이디
            public int x, y; // 좌표
            public int rot; // 회전
        }
        [Serializable]
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
                userId = 0;
                mapId = 0;
            }
        }

        public void SetPlace(ObjectInfo objectInfo)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/엔드포인트";
            info.body = JsonUtility.ToJson(new SetPlaceInfo(objectInfo));
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print("Set요청완료");
            };
            StartCoroutine(httpManager.Post(info));
        }
        [Serializable]
        public class GetPlaceReqInfo
        {
            int userId;
            int mapId;
            public GetPlaceReqInfo()
            {
                // 구현 필요
                userId = 0;
                mapId = 0;
            }
        }
        [Serializable]
        public class GetPlaceResInfo
        {
            public List<ObjectInfo> data;
        }
        public void GetPlace()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/엔드포인트";
            info.body = JsonUtility.ToJson(new GetPlaceReqInfo());
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                GetPlaceResInfo dataInfo = JsonUtility.FromJson<GetPlaceResInfo>(res.text);
                print("Get요청완료 : " + res.text);
            };
            StartCoroutine(httpManager.Get(info));
        }
    }
}
