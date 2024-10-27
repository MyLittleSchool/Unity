using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using GH;

public class PlayerMalpung : MonoBehaviourPun, IPunObservable
{
    //말풍선
    public GameObject malpungPanel;
    public TMP_Text malpungText;
    public TMP_Text playerNameText;
    private string playerName;
    private string playerNamePun;

    //말풍선 시간
    public float currtMalpungTime = 5.0f;
    private float maxMalpungTime = 5.0f;

    bool onMalpung = true;
    //bool onMalpung_Pun;

    void Start()
    {
        //말풍선 끄기
        malpungPanel.SetActive(false);

        // 말풍 텍스트 초기화
        malpungText.text = "";

        //if (photonView.IsMine)
        //{
        //    playerNameText.text = playerName;
        //}
        //else
        //{
        //    playerNameText.text = playerNamePun;
        //}
        playerName = DataManager.instance.playerName;

        Invoke("RPC_NameText", 0.2f);

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
            stream.SendNext(playerName);
        }
        //그렇지 않고 만일 데이터를 서버로부터 읽어오는 상태라면
        else if (stream.IsReading)
        {
            playerNamePun = (string)stream.ReceiveNext();
        }
    }

    public void RPC_MalPungText(string value)
    {
        photonView.RPC("MalPungText", RpcTarget.All, value);
    }



    [PunRPC]
    public void NameText()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = playerName;
        }
        else
        {
            playerNameText.text = playerNamePun;
        }
    }

    public void RPC_NameText()
    {
        photonView.RPC("NameText", RpcTarget.All);
    }
}
