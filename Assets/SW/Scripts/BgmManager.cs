using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public class BgmManager : MonoBehaviour
    {
        public static BgmManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}