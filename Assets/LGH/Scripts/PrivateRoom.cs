using MJ;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using SW;

namespace GH
{

    public class PrivateRoom : MonoBehaviourPunCallbacks, IPunObservable
    {
        public List<GameObject> playersList = new List<GameObject>();
        private GameObject passWordPanel;

        private PrivateRoomPanel privateRoomPanel;

        //룸 패스워드
        public string roomPassword = "99999";
        //룸 패스워드 인풋 필드
        private TMP_InputField passWordInputField;

        //룸 패스워드 안내 텍스트
        private TMP_Text passWordText;

        // 패스워드 틀렸을 때 텍스트
        private TMP_Text passWordWrongText;

        //패스워드 제출 버튼
        private Button passWordSubmitButton;
        private Button passWordExitButton;

        private BoxCollider2D boxCollider;

        public bool activeRoom = false;

        public GameObject playerMine;

        private GameObject darkSprite;

        public int roomNum;

        // 프라이빗 룸 선별(임시)
        public bool playerCheck = false;

        Vector3 enterPosition;

        void Start()
        {
            passWordPanel = GameObject.Find("UIManager").GetComponent<SceneUIManager>().privateRoomPanel;
            privateRoomPanel = passWordPanel.GetComponent<PrivateRoomPanel>();

            passWordInputField = privateRoomPanel.passWordInputField;
            passWordText = privateRoomPanel.passWordText;
            passWordWrongText = privateRoomPanel.passWordWrongText;
            passWordSubmitButton = privateRoomPanel.passWordSubmitButton;
            passWordExitButton = privateRoomPanel.passWordExitButton;

            passWordSubmitButton.onClick.AddListener(PassWordCheck);
            passWordExitButton.onClick.AddListener(PassWordExit);
            boxCollider = GetComponent<BoxCollider2D>();
            darkSprite = transform.GetChild(0).gameObject;
           // StartCoroutine(PlayerSuch());
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (!PhotonNetwork.IsMasterClient) photonView.RPC(nameof(ReqSync), PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        [PunRPC]
        public void ReqSync(int actorNumber)
        {
            photonView.RPC(nameof(ResSync), PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), roomPassword);
        }
        [PunRPC]
        public void ResSync(string password)
        {
            roomPassword = password;
        }

        private void OnUI()
        {
            passWordInputField.text = "";
            print(passWordPanel.name);
            passWordPanel.SetActive(true);

            if (activeRoom == false)
            {
                passWordText.text = "비밀번호 설정해주세요";
            }
            else
            {
                passWordText.text = "비밀번호를 입력해주세요";
            }
        }

        [PunRPC]
        public void PassWordUpload(string s)
        {
            PrivateRoomManager.instance.privateRooms[roomNum].roomPassword = s;
        }

        public void PassWordCheck()
        {
            if (playersList.Count == 1)
            {
               
                if (playerCheck)
                {
                    photonView.RPC(nameof(PassWordUpload), RpcTarget.All, passWordInputField.text);
                    if (playerMine.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        passWordPanel.SetActive(false);

                        playerMine.transform.position = gameObject.transform.position - new Vector3(0, 1.5f, 0);
                        playerCheck = false;

                        // 입장
                        SceneUIManager.GetInstance().OnVoicePanel();
                    }
                }


            }
            if (activeRoom)
            {
                if (playerMine.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    if (passWordInputField.text == roomPassword)
                    {
                        passWordPanel.SetActive(false);
                        playerCheck = false;
                        playerMine.transform.position = gameObject.transform.position - new Vector3(0, 1.5f, 0);

                        // 입장
                        SceneUIManager.GetInstance().OnVoicePanel();

                    }
                    else
                    {

                        passWordWrongText.gameObject.SetActive(true);

                    }
                }

            }

        }

        public void PassWordExit()
        {
            passWordPanel.SetActive(false);
            if (playerCheck)
            {
                if (playerMine.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    darkSprite.SetActive(false);
                    playerMine.transform.position = gameObject.transform.position - new Vector3(0, 4f, 0);
                    playerMine = null;
                    playerCheck = false;
                    darkSprite.SetActive(false);

                    if (playersList.Count == 1)
                    {
                    }
                    else
                    {

                    }
                }


            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    OnUI();
                    darkSprite.SetActive(true);
                    playerMine = collision.gameObject;
                    playerCheck = true;
                    enterPosition = collision.transform.position;
                }
                playersList.Add(collision.gameObject);
                VoiceManager.GetInstance().SettingPlayerList(playersList);
                activeRoom = true;

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i] == collision.gameObject)
                    {
                        playersList.RemoveAt(i);
                        VoiceManager.GetInstance().SettingPlayerList(playersList);
                        if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                        {
                            darkSprite.SetActive(false);
                            playerMine = null;
                            // 퇴장
                            SceneUIManager.GetInstance().OffVoicePanel();
                            VoiceManager.GetInstance().HeadSetOnOff(false);
                            VoiceManager.GetInstance().MicrophoneOnOff(false);
                        }
                        break;
                    }
                }
                if (playersList.Count == 0)
                {
                    activeRoom = false;
                }

            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (collision.gameObject.GetPhotonView().IsMine)
                {
                    if (playerCheck)
                    {
                        collision.transform.position = enterPosition;
                    }
                }
            }

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

        IEnumerator PlayerSuch()
        {
            yield return new WaitForSeconds(2.0f);
            Collider2D[] players = Physics2D.OverlapBoxAll(transform.position, new Vector2(6, 6), 0, 1 << LayerMask.NameToLayer("Player"));
            foreach (Collider2D player in players)
            {
                playersList.Add(player.gameObject);
            }

            if (playersList.Count > 0)
            {
                activeRoom = true;
            }
        }

    }

}