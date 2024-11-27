using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class ShopNPC : Interactive
    {
        public override void Interact()
        {
            SceneUIManager.GetInstance().OnShopPanel();
        }
    }
}