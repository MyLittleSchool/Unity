using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoQuizSquar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            SceneMgr.instance.QuizSquareIn();
        }
    }
}
