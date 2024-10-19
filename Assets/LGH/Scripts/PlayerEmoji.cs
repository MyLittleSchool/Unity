using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{

    public class PlayerEmoji : MonoBehaviour
    {
        public List<GameObject> emojiPrefabList = new List<GameObject>();

        void Start()
        {

        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnEmoji(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnEmoji(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OnEmoji(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                OnEmoji(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                OnEmoji(5);
            }
        }
        private void OnEmoji(int num)
        {
            GameObject emoji = Instantiate(emojiPrefabList[num - 1]);
            emoji.transform.position = transform.position + transform.up;
        }

    }

}