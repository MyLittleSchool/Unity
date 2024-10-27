using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace GH
{
    public class GameManager : MonoBehaviourPun
    {
        public static GameManager instance;

        public GameObject activateButtonPannel;
        public GameObject emojiButtonPannel;
        public Button stingButton;
        // 패널 온 오프
        bool onActivate = true;

        public VariableJoystick Joystick;
        public Transform emojiTransform;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            activateButtonPannel.SetActive(false);
            emojiButtonPannel.SetActive(true);

            StartCoroutine(SpawnPlayer());

            // OnPhotonSerializeView 에서 데이터 전송 빈도 수 설정하기(per seconds)
            PhotonNetwork.SerializationRate = 30;
            // 대부분 데이터 전송 빈도 수 설정하기
            PhotonNetwork.SendRate = 30;
        }
        void Update()
        {
                
            
        }
        IEnumerator SpawnPlayer()
        {
            //룸에 입장이 완료될 때까지 기다린다.
            yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });

            Vector2 randomPos = Random.insideUnitCircle * 5.0f;
            Vector3 initPosition = new Vector3(randomPos.x, randomPos.y, 0);

            PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);
        }
        public void ConversionPanel()
        {
            if (onActivate)
            {
                activateButtonPannel.SetActive(true);
                emojiButtonPannel.gameObject.SetActive(false);
                onActivate = false;
            }
            else
            {
                activateButtonPannel.SetActive(false);
                emojiButtonPannel.gameObject.SetActive(true);
                onActivate = true;

            }
        }
    }
}