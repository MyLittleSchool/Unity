using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Unity.VisualScripting;
using GH;
using Photon.Pun;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance;

    
    private Photon.Voice.Unity.Recorder record;
    private List<Speaker> playerSpeakers = new List<Speaker>();
    bool bSpeaker = true;
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

    // Start is called before the first frame update
    void Start()
    {
        record = GetComponent<Photon.Voice.Unity.Recorder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MicrophoneOnOff()
    {
        record.TransmitEnabled = !record.TransmitEnabled;
    }

    public bool GetMicrophoneOnOff()
    {
        return record.TransmitEnabled;
    }

    public void HeadSetOnOff()
    {
        SettingPlayerSpeaker();

        bSpeaker = !bSpeaker;
        foreach (Speaker speaker in playerSpeakers)
            speaker.enabled = bSpeaker;
        if (!bSpeaker)
            record.TransmitEnabled = false;
    }

    public bool GetHeadSetOnOff()
    {
        return bSpeaker;
    }

    public void SettingPlayerSpeaker()
    {
        playerSpeakers.Clear();
        foreach (PhotonView photonview in PhotonNetwork.PhotonViews)
        {
            if(photonview.gameObject)
                playerSpeakers.Add(photonview.gameObject.GetComponent<Speaker>());
        }
    }

}
