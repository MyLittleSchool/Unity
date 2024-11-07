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
    private List<AudioSource> playerAudioSources = new List<AudioSource>();
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
        foreach (AudioSource audioSource in playerAudioSources)
            audioSource.mute = bSpeaker;
        if (!bSpeaker)
            record.TransmitEnabled = false;
    }

    public bool GetHeadSetOnOff()
    {
        return bSpeaker;
    }

    public void SettingPlayerSpeaker()
    {
        playerAudioSources.Clear();
        foreach (PhotonView photonview in PhotonNetwork.PhotonViews)
        {
            if(photonview.gameObject)
                playerAudioSources.Add(photonview.gameObject.GetComponent<AudioSource>());
        }
    }

}
