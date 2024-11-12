using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using SW;

namespace GH
{
    public class GameManager : MonoBehaviourPun
    {
        public static GameManager instance;

        public Image conversionImage;
        public Sprite activateOn;
        public Sprite activateOff;
        public GameObject emojiButtonPannel;
        public Button stingButton;
        // 패널 온 오프
        bool onActivate = true;

        public VariableJoystick Joystick;
        public Transform emojiTransform;

        public GameObject interacterPrefab;

        public TMP_Text currtRoomPlayerCnt;

        public List<GameObject> emojiList = new List<GameObject>();

        public GameObject interracBut;

        public bool interMode = false;
        public bool buttonDown = false;

        public GameObject firstLoginPanel;
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

        void Start()
        {
            emojiButtonPannel.SetActive(true);
            interracBut.SetActive(false);
            CoSpwamPlayer();
            // OnPhotonSerializeView 에서 데이터 전송 빈도 수 설정하기(per seconds)
            PhotonNetwork.SerializationRate = 60;
            // 대부분 데이터 전송 빈도 수 설정하기
            PhotonNetwork.SendRate = 60;
        }
        void Update()
        {
            if (PhotonNetwork.CurrentRoom != null)
                currtRoomPlayerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            InteractionButton();
        }

        public void CoSpwamPlayer()
        {
            StartCoroutine(SpawnPlayer());
            
        }
        IEnumerator SpawnPlayer()
        {
            //룸에 입장이 완료될 때까지 기다린다.
            yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });

            Vector3 initPosition = new Vector3(0, 0, 0);

            GameObject spwanPlayer = PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);
            Instantiate(interacterPrefab, spwanPlayer.transform);

            if (spwanPlayer.GetComponent<PhotonView>().IsMine)
            {
                DataManager.instance.player = spwanPlayer;
            }

        }
        public void ConversionPanel()
        {
            if (onActivate)
            {
                conversionImage.sprite = activateOff;
                emojiButtonPannel.gameObject.SetActive(false);
                onActivate = false;
            }
            else
            {
                conversionImage.sprite = activateOn;
                emojiButtonPannel.gameObject.SetActive(true);
                onActivate = true;

            }
        }

        public void InteractionButton()
        {
            if (DataManager.instance != null)
            {

                interracBut.SetActive(interMode);
            }
        }

        public void ClickInterractionButton()
        {
            StartCoroutine(nameof(ButtonUp));
            //버튼 눌렀을 때 상호작용
            DataManager.instance.player.GetComponentInChildren<PlayerInteracter>().InteractBut();

        }
        IEnumerable ButtonUp()
        {
            buttonDown = true;
            yield return null;
            buttonDown = false;
        }

        public void OnTile()
        {
            SetTile setTile = DataManager.instance.player.GetComponent<SetTile>();
            if (setTile.tileObjCheck)
            {
                setTile.DeleteTile();
            }
            else
            {
                setTile.OnTile();
            }
        }


    }
}