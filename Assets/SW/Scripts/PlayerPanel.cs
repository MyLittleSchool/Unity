using GH;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace SW
{
    public class PlayerPanel : MonoBehaviour
    {
        public UserImage userImage;
        public UserRPC userRPC;
        public Button reportButton;
        public void OnFriendReqButtonClick()
        {
            WebSocketManager.GetInstance().OnRequestFriendPanel(userRPC.userInfo.id);
        }
    }
}