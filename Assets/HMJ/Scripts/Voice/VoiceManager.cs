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
    bool bPlay = false;
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

    public static VoiceManager GetInstance()
    {
        return instance;
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

    public void MicrophoneOnOff(bool _onMicro)
    {
        record.TransmitEnabled = _onMicro;
    }

    public bool GetMicrophoneOnOff()
    {
        return record.TransmitEnabled;
    }

    public void HeadSetOnOff(bool _bOn)
    {
        SettingPlayerSpeaker();

        bPlay = _bOn;
        foreach (AudioSource audioSource in playerAudioSources)
            audioSource.mute = !bPlay;
    }

    public void SettingPlayerSpeaker()
    {
        playerAudioSources.Clear();
        foreach (PhotonView photonview in PhotonNetwork.PhotonViews)
        {
            GameObject photonviewObject = photonview.gameObject;
            if (photonviewObject && photonviewObject.GetComponent<AudioSource>())
                playerAudioSources.Add(photonviewObject.GetComponent<AudioSource>());
        }
    }

    public void MoveScene()
    {
        StartCoroutine(MicHeadSetOff());
    }

    public IEnumerator MicHeadSetOff()
    {
        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });
        VoiceManager.GetInstance().HeadSetOnOff(false);
        VoiceManager.GetInstance().MicrophoneOnOff(false);
    }

    public void SettingPlayerList(List<GameObject> playerList)
    {
        // ResetTargetPlayerData();
        int playerActorNumber = DataManager.instance.player.GetPhotonView().OwnerActorNr;

        List<int> ActorList = new List<int>();
        ActorList.Add(0);
        foreach (GameObject ObjectData in playerList)
        {
            int objectActorNumber = ObjectData.GetPhotonView().OwnerActorNr;
            if (objectActorNumber != playerActorNumber)
                ActorList.Add(objectActorNumber);
        }
        int[] targetPlayers = ActorList.ToArray();
        record.TargetPlayers = targetPlayers;
    }
}
