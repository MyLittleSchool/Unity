using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SW
{
    public class PostManager : MonoBehaviour
    {
        private static PostManager instance;
        public static PostManager GetInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "PostManager";
                go.AddComponent<PostManager>();
            }
            return instance;
        }
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
            httpManager = HttpManager.GetInstance();
        }
        private HttpManager httpManager;
        
    }
}
