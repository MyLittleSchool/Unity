using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{

    public class PlayerEmoji : MonoBehaviour
    {
        public List<GameObject> emojiPrefabList = new List<GameObject>();
        public GameObject stingPrefab;

        //찌르기 방향 값
        private Vector3 stingDir;

        void Start()
        {

        }
        void Update()
        {
            // 감정 이모지 생성
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

            // 찌르기 이모지 방향값 설정
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                stingDir = transform.right;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                stingDir = -transform.right;

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                stingDir = transform.up;

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                stingDir = -transform.up;

            }

            //찌르기 이모지 생성
            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnString(stingDir);
            }
        }
        private void OnEmoji(int num)
        {
            GameObject emoji = Instantiate(emojiPrefabList[num - 1]);
            emoji.transform.position = transform.position + transform.up;
        }

        private void OnString(Vector3 dir)
        {
            GameObject sting = Instantiate(stingPrefab);
            sting.transform.position = transform.position;
            sting.transform.right = dir;
        }

    }

}