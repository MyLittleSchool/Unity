using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class DumInteractObj : Interactive
    {
        public override void Interact()
        {
            print(interacterName);
        }
    }
}