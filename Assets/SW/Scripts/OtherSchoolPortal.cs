using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class OtherSchoolPortal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            SceneUIManager.GetInstance().OnVisitOtherSchoolPanel();
        }
    }
}