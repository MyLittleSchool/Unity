using GH;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySting : MonoBehaviour
{
    void Start()
    {
      //  GetComponent<Button>().onClick.AddListener(OnSting);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnSting()
    {
        //DataManager.instance.player.GetComponent<PlayerEmoji>().RPC_OnSting(DataManager.instance.player.GetComponent<PlayerMove>().stingDir);

    }
}
