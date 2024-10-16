using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PostManager : MonoBehaviour
    {
        HttpManager httpManager = HttpManager.GetInstance();
        [Serializable]
        public class PostInfo
        {
            public int userId;
            public int mapId;
            public string nickname;
            public string title;
            public string content;
            public PostInfo()
            {
                // 구현 필요
                userId = 0;
                mapId = 0;
                nickname = "";
            }
        }
        [Serializable]
        public class LoadReqInfo
        {
            int mapId;
            public LoadReqInfo()
            {
                // 구현 필요
                mapId = 0;
            }
        }
        [Serializable]
        public class PostList
        {
            public List<PostInfo> data;
        }
        public void CreatePost(PostInfo postInfo)
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/엔드포인트";
            info.body = JsonUtility.ToJson(postInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                print("게시 요청 완료");
            };
            StartCoroutine(httpManager.Post(info));
        }
        public void LoadPost()
        {
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/엔드포인트";
            info.body = JsonUtility.ToJson(new LoadReqInfo());
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler res) =>
            {
                PostList list = JsonUtility.FromJson<PostList>(res.text);
                print("로딩완료 : " + res.text);
            };
            StartCoroutine(httpManager.Get(info));
        }
    }
}
