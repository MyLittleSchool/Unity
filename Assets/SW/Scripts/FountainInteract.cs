using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SW
{
    public class FountainInteract : Interactive
    {

        protected override void Start()
        {
            base.Start();

        }
        public override void Interact()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                SceneMgr.instance.SquareIn();

            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                SceneMgr.instance.SchoolIn();

            }
            print("분수상호작용");
        }
    }
}