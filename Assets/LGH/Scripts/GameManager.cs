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
        private VerticalLayoutGroup emojiButPanelVerticalLayoutGroup;
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
            emojiButPanelVerticalLayoutGroup = emojiButtonPannel.GetComponent<VerticalLayoutGroup>();
            emojiRT = emojiButtonPannel.GetComponent<RectTransform>();
            stingRT = stingButton.GetComponent<RectTransform>();
            interracBut.SetActive(false);
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
            Vector3 initPosition = GameObject.Find("SpwanPosition").transform.position;

            GameObject spwanPlayer = PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);
            Instantiate(interacterPrefab, spwanPlayer.transform.position + Vector3.up * 0.5f, Quaternion.identity, spwanPlayer.transform);

            if (spwanPlayer.GetComponent<PhotonView>().IsMine)
            {
                DataManager.instance.player = spwanPlayer;
            }

        }

        public void coSpawnSoccer()
        {
            StartCoroutine(SpawnSoccer());

        }
        IEnumerator SpawnSoccer()
        {
            //룸에 입장이 완료될 때까지 기다린다.
            yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });

            Vector3 initPosition = GameObject.Find("Objects/SoccerPosition").transform.position;

            GameObject spawnSoccer = PhotonNetwork.Instantiate("Soccer", initPosition, Quaternion.identity);
        }
        public void ConversionPanel()
        {
            if (onActivate)
            {
                conversionImage.sprite = activateOff;
                //emojiButtonPannel.gameObject.SetActive(false);
                onActivate = false;
                iTween.Stop(emojiButtonPannel);
                MoveEmojiButton(stingRT.anchoredPosition.y + 150);
                emojiButPanelVerticalLayoutGroup.spacing = 15;
                iTween.ValueTo(emojiButtonPannel, iTween.Hash(
                    "from", stingRT.anchoredPosition.y + 150,
                    "to", stingRT.anchoredPosition.y,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutQuint,
                    "onupdate", nameof(MoveEmojiButton),
                    "onupdatetarget", gameObject
                ));
                iTween.ValueTo(emojiButtonPannel, iTween.Hash(
                    "from", 15,
                    "to", -130,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutQuint,
                    "onupdate", nameof(ChangeSpacing),
                    "onupdatetarget", gameObject
                ));
            }
            else
            {
                conversionImage.sprite = activateOn;
                //emojiButtonPannel.gameObject.SetActive(true);
                onActivate = true;
                iTween.Stop(emojiButtonPannel);
                MoveEmojiButton(stingRT.anchoredPosition.y);
                emojiButPanelVerticalLayoutGroup.spacing = -130;
                iTween.ValueTo(emojiButtonPannel, iTween.Hash(
                    "from", stingRT.anchoredPosition.y,
                    "to", stingRT.anchoredPosition.y + 150,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutQuint,
                    "onupdate", nameof(MoveEmojiButton),
                    "onupdatetarget", gameObject
                ));
                iTween.ValueTo(emojiButtonPannel, iTween.Hash(
                    "from", -130,
                    "to", 15,
                    "time", 0.6f,
                    "easetype", iTween.EaseType.easeOutQuint,
                    "onupdate", nameof(ChangeSpacing),
                    "onupdatetarget", gameObject
                ));
            }
        }
        RectTransform stingRT;
        RectTransform emojiRT;
        public void ChangeSpacing(float newValue)
        {
            emojiButPanelVerticalLayoutGroup.spacing = newValue;
        }
        public void MoveEmojiButton(float newValue)
        {
            emojiRT.anchoredPosition = new Vector3(emojiRT.anchoredPosition.x, newValue, 0);
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
                setTile.RPC_DeleteTile();
            }
            else
            {
                setTile.OnTile();
            }
        }


    }
}