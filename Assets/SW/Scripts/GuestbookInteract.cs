using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class GuestbookInteract : Interactive
    {
        private SceneUIManager sceneUIManager;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            sceneUIManager = GameObject.Find("SceneUIManager").GetComponent<SceneUIManager>();
        }
        public override void Interact()
        {
            sceneUIManager.OnGuestbookPanel();
        }

    }
}