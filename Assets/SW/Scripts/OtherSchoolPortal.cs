using MJ;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class OtherSchoolPortal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine) SceneUIManager.GetInstance().OnVisitOtherSchoolPanel();
        }
    }
}