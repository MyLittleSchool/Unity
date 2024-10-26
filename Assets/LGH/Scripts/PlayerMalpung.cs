using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerMalpung : MonoBehaviourPun, IPunObservable
{
    //말풍선
    public GameObject malpungPanel;
    public TMP_Text malpungText;


    //말풍선 시간
    public float currtMalpungTime = 5.0f;
    private float maxMalpungTime = 5.0f;

    bool onMalpung = true;
    bool onMalpung_Pun;
    void Start()
    {
        //말풍선 끄기
        malpungPanel.SetActive(false);

        // 말풍 텍스트 초기화
        malpungText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        OnMalpung();
        malpungPanel.SetActive(onMalpung);

    }
    //말풍선 생기기
    private void OnMalpung()
    {
        currtMalpungTime += Time.deltaTime;
        if (currtMalpungTime < maxMalpungTime)
        {
            onMalpung = true;
        }
        else
        {
            //malpungPanel.SetActive(false);
            onMalpung = false;
        }



    }

    [PunRPC]
    public void MalPungText(string value)
    {
        malpungText.text = value;
        currtMalpungTime = 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만일 데이터를 서버에 전송(PhotonView.IsMine == true)하는 상태라면
        if (stream.IsWriting)
        {
            stream.SendNext(onMalpung);

        }
        //그렇지 않고 만일 데이터를 서버로부터 읽어오는 상태라면
        else if (stream.IsReading)
        {
            //위에 받는 순서대로 변수를 캐스팅 해줘야 한다.
            onMalpung_Pun = (bool)stream.ReceiveNext();
        }
    }

    public void RPC_MalPungText(string value)
    {
        photonView.RPC("MalPungText", RpcTarget.All, value);
    }
}
