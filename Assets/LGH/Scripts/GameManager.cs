using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace GH
{
    public class GameManager : MonoBehaviourPun
    {
        public GameObject activateButtonPannel;
        public GameObject emojiButtonPannel;
        // 패널 온 오프
        bool onActivate = true;


        void Start()
        {
            activateButtonPannel.SetActive(false);
            emojiButtonPannel.SetActive(true);

            StartCoroutine(SpawnPlayer());
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
            print("aa");
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