using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW {
    public class ArchiveInteract : Interactive
    {
        public override void Interact()
        {
            SceneUIManager.GetInstance().OnArchivePanel();
        }
    }
}