using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class GuestbookInteract : Interactive
    {
        private UIManager uiManager;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }
        public override void Interact()
        {
            // uiManager.OnGuestbookPanel();
        }

    }
}