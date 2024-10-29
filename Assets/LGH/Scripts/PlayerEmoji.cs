using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class PlayerEmoji : MonoBehaviourPun, IPunObservable
    {
        public List<GameObject> emojiPrefabList = new List<GameObject>();
        public GameObject stingPrefab;

        public Transform emojiTransform;
        public GameObject emojiButtonPrefab;


        //찌르기 방향 값
        private Vector3 stingDir;

        PlayerMove playerMove;


        void Start()
        {
            playerMove = GetComponent<PlayerMove>();

            emojiTransform = GameManager.instance.emojiTransform;

            if(photonView.IsMine && GameManager.instance.emojiList.Count != 0)
            {
                for(int i = 0; i < GameManager.instance.emojiList.Count; i++)
                {
                    Destroy(GameManager.instance.emojiList[i].gameObject);
                }
                GameManager.instance.emojiList.Clear();
            }
            if (GetComponent<PhotonView>().IsMine)
            {
                //이모지 버튼 생성
                for (int i = 0; i < emojiPrefabList.Count; i++)
                {
                    GameObject emoji = Instantiate(emojiButtonPrefab, emojiTransform);
                    EmojiButton emojiBut = emoji.GetComponent<EmojiButton>();
                    emojiBut.EmojiIndex(i);
                    emojiBut.playerEmoji = gameObject.GetComponent<PlayerEmoji>();
                    Image emojiImage = emoji.transform.GetChild(0).gameObject.GetComponent<Image>();
                    emojiImage.sprite = emojiPrefabList[i].GetComponent<SpriteRenderer>().sprite;
                    GameManager.instance.emojiList.Add(emoji);
                }
            }
            if (photonView.IsMine)
            {
                GameManager.instance.stingButton.onClick.AddListener(RPC_OnSting);

            }

        }
        void Update()
        {
            #region 컴퓨터 감정 표현
            /*
            if (GetComponent<PhotonView>().IsMine)
            {
                // 감정 이모지 생성
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    OnEmoji(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    OnEmoji(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    OnEmoji(2);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    OnEmoji(3);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    OnEmoji(4);
                }



                //찌르기 이모지 생성
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    OnString();
                }
            }
            */
            #endregion
        }
        [PunRPC]
        public void OnEmoji(int num)
        {

            GameObject emoji = Instantiate(emojiPrefabList[num]);
            emoji.transform.position = transform.position + (transform.up * 1.5f);

        }
        public void RPC_OnEmoji(int num)
        {
            photonView.RPC(nameof(OnEmoji), RpcTarget.All, num);
        }

        [PunRPC]
        public void OnString()
        {
            if (photonView.IsMine)
            {
                stingDir = GetComponent<PlayerMove>().stingDir;

            }
            else
            {
                stingDir = GetComponent<PlayerMove>().stingDirPun;

            }
            GameObject sting = Instantiate(stingPrefab);
            sting.transform.position = transform.position;
            sting.transform.right = stingDir;

        }

        public void RPC_OnSting()
        {

            photonView.RPC(nameof(OnString), RpcTarget.All);

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // 만일 데이터를 서버에 전송(PhotonView.IsMine == true)하는 상태라면
            if (stream.IsWriting)
            {

            }
            //그렇지 않고 만일 데이터를 서버로부터 읽어오는 상태라면
            else if (stream.IsReading)
            {
                //위에 받는 순서대로 변수를 캐스팅 해줘야 한다.
            }
        }
    }

}