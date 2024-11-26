using MJ;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoQuizSquare : MonoBehaviour
{
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            SceneUIManager.GetInstance().OffQuizCategoryPanel();
            SceneUIManager.GetInstance().OffQuizQuestionPanel();
            SceneMgr.instance.QuizSquareIn();
        }
    }
}
