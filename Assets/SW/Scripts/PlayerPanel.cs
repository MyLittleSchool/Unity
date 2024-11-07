using GH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace SW
{
    public class PlayerPanel : MonoBehaviour
    {
        public UserRPC userRPC;
        public void OnFriendReqButtonClick()
        {
            int myId = AuthManager.GetInstance().userAuthData.userInfo.id;
            int targetId = userRPC.userInfo.id;
            HttpManager httpManager = HttpManager.GetInstance();
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/friendship/request?requesterId=" + myId + "&receiverId=" + targetId;
            info.onComplete = (DownloadHandler res) =>
            {
                print(res.text);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
    }
}