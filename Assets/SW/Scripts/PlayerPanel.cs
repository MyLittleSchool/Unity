using GH;
using System;
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
            WebSocketManager.GetInstance().RequestFriend(userRPC.userInfo.id);
        }
    }
}