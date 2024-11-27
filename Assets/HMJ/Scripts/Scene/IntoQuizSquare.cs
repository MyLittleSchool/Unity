using GH;
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
            SceneMgr.instance.QuizSquareIn();

            StartCoroutine(WaitInRoom_Quest());
            //QuestManager.instance.QuestPatch(5);
        }
    }

    IEnumerator WaitInRoom_Quest()
    {
        yield return new WaitForSeconds(0.2f);
        QuestManager.instance.QuestPatch(5);
    }
}
