using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class FountainInteract : Interactive
    {

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

        }
        public override void Interact()
        {
            print("분수상호작용");
        }
    }
}