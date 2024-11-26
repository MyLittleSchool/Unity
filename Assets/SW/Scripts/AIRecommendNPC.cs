using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class AIRecommendNPC : Interactive
    {
        public override void Interact()
        {
            SceneUIManager.GetInstance().OnFriendsPanel();
            StartCoroutine(NextFrame());
        }
        IEnumerator NextFrame()
        {
            yield return null;
            WebSocketManager.GetInstance().friendsUI.ChangeTab(3);
        }
    }
}